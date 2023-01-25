namespace Moosesoft.Azure.Messaging.ServiceBus;

internal class FuncMessageProcessor : IMessageProcessor
{
    private readonly Func<ServiceBusReceivedMessage, CancellationToken, Task> _processMessage;

    public FuncMessageProcessor(Func<ServiceBusReceivedMessage, CancellationToken, Task> processMessage)
    {
        _processMessage = processMessage ?? throw new ArgumentNullException(nameof(processMessage));
    }

    public async Task ProcessMessageAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken) 
        => await _processMessage(message, cancellationToken).ConfigureAwait(false);

}