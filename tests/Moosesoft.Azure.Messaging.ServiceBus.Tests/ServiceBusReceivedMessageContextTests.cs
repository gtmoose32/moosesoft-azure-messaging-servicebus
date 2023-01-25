// ReSharper disable ExpressionIsAlwaysNull

namespace Moosesoft.Azure.Messaging.ServiceBus.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class ServiceBusReceivedMessageContextTests
{
    private ServiceBusReceivedMessage _message;
    private ServiceBusReceiver _receiver;
    private ServiceBusClient _client;
    private ServiceBusReceivedMessageContext _sut;

    [TestInitialize]
    public void Init()
    {
        _message = ServiceBusReceivedMessageFactory.Create();
        _receiver = Substitute.For<ServiceBusReceiver>();
        _client = Substitute.For<ServiceBusClient>();
        _sut = new ServiceBusReceivedMessageContext(_message, _receiver, _client);
    }

    [TestMethod]
    public void CreateMessageSender_Test()
    {
        //Arrange
        var entityDescription = new ServiceBusEntityDescription("test-queue");

        //Act
        var result = _sut.CreateMessageSender(entityDescription);

        //Assert
        result.Should().NotBeNull();

        _client.Received(1).CreateSender(Arg.Is(entityDescription.QueueName));
    }

    [TestMethod]
    public void CreateMessageSender_ArgumentNull_Test()
    {
        //Arrange
        ServiceBusEntityDescription entityDescription = null;

        //Act
        Action act = () => _sut.CreateMessageSender(entityDescription);

        //Assert
        act.Should().ThrowExactly<ArgumentNullException>();

        _client.DidNotReceiveWithAnyArgs().CreateSender(Arg.Any<string>());
    }

    [TestMethod]
    public void ToServiceBusMessage_Test()
    {
        //Arrange

        //Act
        var result = _sut.ToServiceBusMessage();

        //Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void DeliveryCount_Test()
    {
        //Arrange

        //Act
        var result = _sut.DeliveryCount;

        //Assert
        result.Should().Be(1);
    }

    [TestMethod]
    public void RetryCount_Test()
    {
        //Arrange

        //Act
        var result = _sut.RetryCount;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public async Task DeadLetterMessageAsync_Test()
    {
        //Arrange
        const string reason = "dead letter reason.";

        //Act
        await _sut.DeadLetterMessageAsync(reason, CancellationToken.None).ConfigureAwait(false);

        //Assert
        await _receiver.Received(1)
            .DeadLetterMessageAsync(Arg.Is(_message), Arg.Is(reason), cancellationToken: Arg.Is(CancellationToken.None))
            .ConfigureAwait(false);
    }

    [TestMethod]
    public async Task CompleteMessageAsync_Test()
    {
        //Arrange

        //Act
        await _sut.CompleteMessageAsync(CancellationToken.None).ConfigureAwait(false);

        //Assert
        await _receiver.Received(1)
            .CompleteMessageAsync(Arg.Is(_message), cancellationToken: Arg.Is(CancellationToken.None))
            .ConfigureAwait(false);
    }

    [TestMethod]
    public async Task AbandonMessageAsync_Test()
    {
        //Arrange
        IDictionary<string, object> properties = new Dictionary<string, object>();

        //Act
        await _sut.AbandonMessageAsync(properties, CancellationToken.None).ConfigureAwait(false);

        //Assert
        await _receiver.Received(1)
            .AbandonMessageAsync(Arg.Is(_message), Arg.Is(properties), cancellationToken: Arg.Is(CancellationToken.None))
            .ConfigureAwait(false);
    }
}