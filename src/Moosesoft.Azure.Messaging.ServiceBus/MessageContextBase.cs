using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.Messaging.ServiceBus;

public abstract class MessageContextBase
{
    private readonly ServiceBusClient _client;

    protected ServiceBusReceivedMessage Message { get; }

    /// <summary>
    /// For unit testing mock-able instances.
    /// </summary>
    protected internal MessageContextBase()
    {
    }

    protected MessageContextBase(ServiceBusReceivedMessage message, ServiceBusClient client)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    [ExcludeFromCodeCoverage]
    public virtual int DeliveryCount => Message.DeliveryCount;

    public virtual int RetryCount => GetRetryCount();

    public virtual ServiceBusSender CreateMessageSender(ServiceBusEntityDescription description)
    {
        if (description == null) throw new ArgumentNullException(nameof(description));

        return _client.CreateSender(description.IsQueue() ? description.QueueName : description.TopicName);
    }

    public abstract Task DeadLetterMessageAsync(string reason, CancellationToken cancellationToken);

    public abstract Task CompleteMessageAsync(CancellationToken cancellationToken);

    public abstract Task AbandonMessageAsync(IDictionary<string, object> propertiesToModify = null, CancellationToken cancellationToken = default);

    public virtual ServiceBusMessage ToServiceBusMessage() => new(Message) { MessageId = Guid.NewGuid().ToString() };

    public static implicit operator ServiceBusReceivedMessage(MessageContextBase messageContext) => messageContext.Message;

    private int GetRetryCount() =>
        Message.ApplicationProperties.TryGetValue(Constants.RetryCountPropertyName, out var count) && count != null
            ? (int)count
            : 0;
}