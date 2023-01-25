namespace Moosesoft.Azure.Webjobs.Extensions.ServiceBus.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class WebJobsServiceBusReceivedMessageContextTests
{
    private ServiceBusMessageActions _messageActions;
    private ServiceBusReceivedMessage _message;
    private ServiceBusClient _client;
    private WebJobsServiceBusReceivedMessageContext _sut;

    [TestInitialize]
    public void Init()
    {
        _messageActions = Substitute.For<ServiceBusMessageActions>();
        _message = ServiceBusReceivedMessageFactory.Create();
        _client = Substitute.For<ServiceBusClient>();
        _sut = new WebJobsServiceBusReceivedMessageContext(_message, _messageActions, _client);
    }

    [TestMethod]
    public async Task DeadLetterMessageAsync_Test()
    {
        //Arrange
        const string reason = "dead letter reason.";

        //Act
        await _sut.DeadLetterMessageAsync(reason, CancellationToken.None).ConfigureAwait(false);
        
        //Assert
        await _messageActions.Received(1)
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
        await _messageActions.Received(1)
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
        await _messageActions.Received(1)
            .AbandonMessageAsync(Arg.Is(_message), Arg.Is(properties), cancellationToken: Arg.Is(CancellationToken.None))
            .ConfigureAwait(false);
    }
}