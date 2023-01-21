using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.Messaging.ServiceBus;

public abstract class ServiceBusReceivedMessageContextBase : IServiceBusReceivedMessageContext
{
    private readonly ServiceBusClient _client;

    /// <inheritdoc />
    public ServiceBusReceivedMessage Message { get; }

    protected ServiceBusReceivedMessageContextBase(ServiceBusReceivedMessage message, ServiceBusClient client)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    /// <inheritdoc />
    public ServiceBusSender CreateMessageSender(ServiceBusEntityDescription description)
    {
        if (description == null) throw new ArgumentNullException(nameof(description));

        return _client.CreateSender(description.IsQueue() ? description.QueueName : description.TopicName);
    }

    /// <inheritdoc />
    public abstract Task DeadLetterMessageAsync(string reason, CancellationToken cancellationToken);

    /// <inheritdoc />
    public abstract Task CompleteMessageAsync(CancellationToken cancellationToken);

    /// <inheritdoc />
    public abstract Task AbandonMessageAsync(IDictionary<string, object> propertiesToModify = null, CancellationToken cancellationToken = default);
}