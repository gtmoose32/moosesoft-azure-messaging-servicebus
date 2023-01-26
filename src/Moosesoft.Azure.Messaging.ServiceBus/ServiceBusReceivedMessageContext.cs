namespace Moosesoft.Azure.Messaging.ServiceBus;

internal class ServiceBusReceivedMessageContext : MessageContext
{
    private readonly ServiceBusReceiver _receiver;

    public ServiceBusReceivedMessageContext(ServiceBusReceivedMessage message, ServiceBusReceiver receiver, ServiceBusClient client)
        : base(message, client)
    {
        _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
    }

    public override async Task DeadLetterMessageAsync(string reason, CancellationToken cancellationToken)
    {
        await _receiver.DeadLetterMessageAsync(Message, reason, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public override async Task CompleteMessageAsync(CancellationToken cancellationToken)
    {
        await _receiver.CompleteMessageAsync(Message, cancellationToken).ConfigureAwait(false);
    }

    public override async Task AbandonMessageAsync(IDictionary<string, object> propertiesToModify = null, CancellationToken cancellationToken = default)
    {
        await _receiver.AbandonMessageAsync(Message, propertiesToModify, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}