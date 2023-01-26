using Moosesoft.Azure.Messaging.ServiceBus.FailurePolicies;

namespace Moosesoft.Azure.Messaging.ServiceBus.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class ServiceBusReceivedMessageContextProcessorTests
{
    private IMessageContextProcessor _sut;
    private IMessageProcessor _messageProcessor;
    private IFailurePolicy _failurePolicy;
    private ServiceBusEntityDescription _entityDescription;

    [TestInitialize]
    public void Init()
    {
        _entityDescription = new ServiceBusEntityDescription("queue");
        _failurePolicy = Substitute.For<IFailurePolicy>();
        _messageProcessor = Substitute.For<IMessageProcessor>();
        _sut = new ServiceBusReceivedMessageContextProcessor(
            _entityDescription, _messageProcessor, _failurePolicy, e => e is InvalidOperationException);
    }

    [TestMethod]
    public void Name_Test()
    {
        //Arrange

        //Act
        var result = _sut.Name;

        //Assert
        result.Should().Be("default");
    }

    [TestMethod]
    public async Task ProcessMessageContextAsync_Test()
    {
        //Arrange
        var messageContext = Substitute.For<MessageContext>();

        //Act
        await _sut.ProcessMessageContextAsync(messageContext, CancellationToken.None).ConfigureAwait(false);

        //Assert
        await _messageProcessor.Received().ProcessMessageAsync(Arg.Any<ServiceBusReceivedMessage>(), Arg.Is(CancellationToken.None))
            .ConfigureAwait(false);

        _failurePolicy.DidNotReceiveWithAnyArgs().CanHandle(Arg.Any<Exception>());
        await _failurePolicy.DidNotReceiveWithAnyArgs()
            .HandleFailureAsync(Arg.Any<MessageContext>(), Arg.Any<CancellationToken>()).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task ProcessMessageContextAsync_Failed_Complete_Test()
    {
        //Arrange
        //var message = CreateMessage();
        var messageContext = Substitute.For<MessageContext>();
        _messageProcessor.ProcessMessageAsync(Arg.Any<ServiceBusReceivedMessage>(), Arg.Any<CancellationToken>())
            .Throws(new InvalidOperationException());

        //Act
        await _sut.ProcessMessageContextAsync(messageContext, CancellationToken.None).ConfigureAwait(false);

        //Assert
        await _messageProcessor.Received(1)
            .ProcessMessageAsync(Arg.Any<ServiceBusReceivedMessage>(), Arg.Is(CancellationToken.None))
            .ConfigureAwait(false);

        //await _messageReceiver.Received().CompleteAsync(Arg.Is(message.SystemProperties.LockToken)).ConfigureAwait(false);
        //await _messageReceiver.DidNotReceiveWithAnyArgs().AbandonAsync(Arg.Any<string>()).ConfigureAwait(false);

        _failurePolicy.DidNotReceiveWithAnyArgs().CanHandle(Arg.Any<Exception>());

        await _failurePolicy.DidNotReceiveWithAnyArgs()
            .HandleFailureAsync(Arg.Any<MessageContext>(), Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }

    [TestMethod]
    public async Task ProcessMessageContextAsync_Failed_Abandon_Test()
    {
        //Arrange
        var messageContext = Substitute.For<MessageContext>();
        _failurePolicy.CanHandle(Arg.Any<Exception>()).Returns(false);
        var exception = new Exception();
        _messageProcessor.ProcessMessageAsync(Arg.Any<ServiceBusReceivedMessage>(), Arg.Any<CancellationToken>())
            .Throws(exception);

        //Act
        await _sut.ProcessMessageContextAsync(messageContext, CancellationToken.None).ConfigureAwait(false);

        //Assert
        await _messageProcessor.Received(1)
            .ProcessMessageAsync(Arg.Any<ServiceBusReceivedMessage>(), Arg.Is(CancellationToken.None))
            .ConfigureAwait(false);

        //await _messageReceiver.DidNotReceiveWithAnyArgs().CompleteAsync(null).ConfigureAwait(false);
        //await _messageReceiver.Received().AbandonAsync(Arg.Is(message.SystemProperties.LockToken)).ConfigureAwait(false);

        _failurePolicy.Received().CanHandle(Arg.Is(exception));
        await _failurePolicy.DidNotReceiveWithAnyArgs()
            .HandleFailureAsync(Arg.Any<MessageContext>(), Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }

    [TestMethod]
    public async Task ProcessMessageContextAsync_FailurePolicy_Handle_Test()
    {
        //Arrange
        var messageContext = Substitute.For<MessageContext>();

        _failurePolicy.CanHandle(Arg.Any<Exception>()).Returns(true);

        var exception = new Exception();
        _messageProcessor.ProcessMessageAsync(Arg.Any<ServiceBusReceivedMessage>(), Arg.Any<CancellationToken>())
            .Throws(exception);

        //Act
        await _sut.ProcessMessageContextAsync(messageContext, CancellationToken.None).ConfigureAwait(false);

        //Assert
        await _messageProcessor.Received(1)
            .ProcessMessageAsync(Arg.Any<ServiceBusReceivedMessage>(), Arg.Is(CancellationToken.None))
            .ConfigureAwait(false);

        //await _messageReceiver.DidNotReceiveWithAnyArgs().CompleteAsync(null).ConfigureAwait(false);
        //await _messageReceiver.DidNotReceiveWithAnyArgs().AbandonAsync(null).ConfigureAwait(false);

        _failurePolicy.Received().CanHandle(Arg.Is(exception));
        await _failurePolicy.Received(1)
            .HandleFailureAsync(Arg.Is(messageContext), Arg.Is(CancellationToken.None))
            .ConfigureAwait(false);
    }
}