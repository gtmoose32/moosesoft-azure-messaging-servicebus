using Azure.Messaging.ServiceBus;
using FluentAssertions;
using Moosesoft.Azure.Messaging.ServiceBus.FailurePolicies;
using NSubstitute;

namespace Moosesoft.Azure.Messaging.ServiceBus.Tests.FailurePolicies;

[ExcludeFromCodeCoverage]
[TestClass]
public class CloneMessageFailurePolicyTests
{
    private CloneMessageFailurePolicy _sut;
    private ServiceBusEntityDescription _entityDescription;

    [TestInitialize]
    public void Init()
    {
        _entityDescription = new ServiceBusEntityDescription("test-queue");
        _sut = new CloneMessageFailurePolicy(ex => ex is InvalidOperationException);
        _sut.SetEntityDescription(_entityDescription);
    }

    [TestMethod]
    public void CanHandle_Test()
    {
        //Act
        var result = _sut.CanHandle(new Exception());
        var result2 = _sut.CanHandle(new InvalidOperationException());

        //Assert
        result.Should().BeFalse();
        result2.Should().BeTrue();
    }

    [TestMethod]
    public async Task HandleFailureAsync_Test()
    {
        //Arrange
        var messageContext = Substitute.For<MessageContextBase>();
        var sender = Substitute.For<ServiceBusSender>();
        messageContext.CreateMessageSender(Arg.Any<ServiceBusEntityDescription>()).ReturnsForAnyArgs(sender);
        messageContext.DeliveryCount.Returns(1);
        messageContext.RetryCount.Returns(0);

        var message = new ServiceBusMessage { MessageId = Guid.NewGuid().ToString()};
        messageContext.ToServiceBusMessage().Returns(message);

        //Act
        await _sut.HandleFailureAsync(messageContext, CancellationToken.None).ConfigureAwait(false);

        //Assert
        await messageContext.Received(1)
            .CompleteMessageAsync(Arg.Is(CancellationToken.None))
            .ConfigureAwait(false);

        messageContext.Received(1).CreateMessageSender(Arg.Is(_entityDescription));

        messageContext.Received(1).ToServiceBusMessage();

        await sender.Received(1)
            .SendMessageAsync(Arg.Is(message))
            .ConfigureAwait(false);

        await messageContext.DidNotReceiveWithAnyArgs()
            .DeadLetterMessageAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }

    [TestMethod]
    public async Task HandleFailureAsync_MaxDeliveryCount_Test()
    {
        //Arrange
        var messageContext = Substitute.For<MessageContextBase>();
        var sender = Substitute.For<ServiceBusSender>();
        messageContext.CreateMessageSender(Arg.Any<ServiceBusEntityDescription>()).ReturnsForAnyArgs(sender);

        messageContext.DeliveryCount.Returns(1);
        messageContext.RetryCount.Returns(9);

        //Act
        await _sut.HandleFailureAsync(messageContext, CancellationToken.None).ConfigureAwait(false);

        //Assert
        await messageContext.DidNotReceiveWithAnyArgs()
            .CompleteMessageAsync(Arg.Any<CancellationToken>())
            .ConfigureAwait(false);

        messageContext.DidNotReceiveWithAnyArgs()
            .CreateMessageSender(Arg.Any<ServiceBusEntityDescription>());

        messageContext.DidNotReceive().ToServiceBusMessage();

        await messageContext.Received(1)
            .DeadLetterMessageAsync(Arg.Any<string>(), Arg.Is(CancellationToken.None))
            .ConfigureAwait(false);
    }
}