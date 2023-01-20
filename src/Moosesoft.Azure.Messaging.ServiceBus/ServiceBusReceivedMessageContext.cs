using Azure.Messaging.ServiceBus;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.Messaging.ServiceBus;

/// <inheritdoc />
public class ServiceBusReceivedMessageContext : IServiceBusReceivedMessageContext
{
    private readonly ServiceBusClient _client;

    private readonly ServiceBusReceiver _receiver;

    /// <inheritdoc />
    public ServiceBusReceivedMessage Message { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    /// <param name="receiver"></param>
    public ServiceBusReceivedMessageContext(ServiceBusClient client, ServiceBusReceiver receiver)
    {
        _client = client;
        _receiver = receiver;
    }

    /// <inheritdoc />
    public ServiceBusSender CreateMessageSender(ServiceBusEntityDescription description)
        => _client.CreateSender(description.QueueName ?? description.TopicName);


    /// <inheritdoc />
    public async Task DeadLetterMessageAsync(ServiceBusReceivedMessage message, string reason,
        CancellationToken cancellationToken)
    {
        await _receiver.DeadLetterMessageAsync(
                message,
                reason,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task CompleteMessageAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken)
    {
        await _receiver.CompleteMessageAsync(message, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task AbandonMessageAsync(ServiceBusReceivedMessage message, IDictionary<string, object> propertiesToModify, CancellationToken cancellationToken)
    {
        await _receiver.AbandonMessageAsync(message, propertiesToModify, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}