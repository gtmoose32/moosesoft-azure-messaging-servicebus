namespace Moosesoft.Azure.Messaging.ServiceBus;

/// <summary>
/// Provides base functionality for message context. This is an abstract class.
/// </summary>
public abstract class MessageContext
{
    private readonly ServiceBusClient _client;

    /// <summary>
    /// Message received from Azure Service Bus for processing.
    /// </summary>
    protected ServiceBusReceivedMessage Message { get; }

    /// <summary>
    /// For unit testing mock-able instances.
    /// </summary>
    protected internal MessageContext()
    {
    }

    /// <summary>
    /// Creates an instance of a message context.
    /// </summary>
    /// <param name="message">Underlying <see cref="ServiceBusReceivedMessage"/> for processing.</param>
    /// <param name="client"><see cref="ServiceBusClient"/> used when necessary to create <see cref="ServiceBusSender"/>.</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected MessageContext(ServiceBusReceivedMessage message, ServiceBusClient client)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    /// <summary>
    /// Received message delivery count.
    /// </summary>
    public virtual int DeliveryCount => Message.DeliveryCount;

    /// <summary>
    /// Received message retry count.
    /// </summary>
    public virtual int RetryCount => GetRetryCount();

    /// <summary>
    /// Creates a new instance of <see cref="ServiceBusSender"/>.
    /// </summary>
    /// <param name="description">The service bus entity destination for message sender.</param>
    /// <returns><see cref="ServiceBusSender"/></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public virtual ServiceBusSender CreateMessageSender(ServiceBusEntityDescription description)
    {
        if (description == null) throw new ArgumentNullException(nameof(description));

        return _client.CreateSender(description.IsQueue() ? description.QueueName : description.TopicName);
    }

    /// <summary>
    /// Sends the underlying <see cref="ServiceBusReceivedMessage"/> to the dead letter queue.
    /// </summary>
    /// <param name="reason">Reason for sending to dead letter queue.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    public abstract Task DeadLetterMessageAsync(string reason, CancellationToken cancellationToken);

    /// <summary>
    /// Completes the underlying <see cref="ServiceBusReceivedMessage"/>.
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    public abstract Task CompleteMessageAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Abandons the underlying <see cref="ServiceBusReceivedMessage"/>.
    /// </summary>
    /// <param name="propertiesToModify">Dictionary of message properties to modify on abandon.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    public abstract Task AbandonMessageAsync(IDictionary<string, object> propertiesToModify = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual ServiceBusMessage ToServiceBusMessage() => new(Message) { MessageId = Guid.NewGuid().ToString() };

    /// <summary>
    /// Implicitly casts any <see cref="MessageContext"/> to <see cref="ServiceBusReceivedMessage"/>.
    /// </summary>
    /// <param name="messageContext"><see cref="MessageContext"/> to cast.</param>
    /// <returns>The underlying <see cref="ServiceBusReceivedMessage"/> contained within the message context.</returns>
    public static implicit operator ServiceBusReceivedMessage(MessageContext messageContext) => messageContext.Message;

    private int GetRetryCount() =>
        Message.ApplicationProperties.TryGetValue(Constants.RetryCountPropertyName, out var count) && count != null
            ? (int)count
            : 0;
}