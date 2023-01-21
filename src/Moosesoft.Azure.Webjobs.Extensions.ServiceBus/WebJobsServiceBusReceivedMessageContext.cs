using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs.ServiceBus;
using Moosesoft.Azure.Messaging.ServiceBus;

namespace Moosesoft.Azure.Webjobs.Extensions.ServiceBus;

/// <summary>
/// <see cref="IServiceBusReceivedMessageContext"/> implementation compatible for use with Microsoft.Azure.WebJobs and AzureFunctions
/// which expose a <see cref="ServiceBusMessageActions"/> class for handling messages.
/// </summary>
public class WebJobsServiceBusReceivedMessageContext : IServiceBusReceivedMessageContext
{

    /// <inheritdoc />
    public ServiceBusReceivedMessage Message { get; }


    private readonly ServiceBusClient _client;
    private readonly ServiceBusMessageActions _messageActions;

    /// <inheritdoc />
    public ServiceBusSender CreateMessageSender(ServiceBusEntityDescription description)
        => _client.CreateSender(description.QueueName ?? description.TopicName);

    /// <summary>
    /// Creates a new <see cref="WebJobsServiceBusReceivedMessageContext"/>
    /// </summary>
    /// <param name="message"></param>
    /// <param name="messageActions"></param>
    /// <param name="client"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public WebJobsServiceBusReceivedMessageContext(ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions, ServiceBusClient client)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        _messageActions = messageActions ?? throw new ArgumentNullException(nameof(messageActions));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    /// <inheritdoc />
    public async Task DeadLetterMessageAsync(ServiceBusReceivedMessage messageContextMessage, string reason, CancellationToken cancellationToken)
    {
        await _messageActions.DeadLetterMessageAsync(messageContextMessage, reason, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task CompleteMessageAsync(ServiceBusReceivedMessage messageContextMessage, CancellationToken cancellationToken)
    {
        await _messageActions.CompleteMessageAsync(messageContextMessage, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task AbandonMessageAsync(ServiceBusReceivedMessage message, IDictionary<string, object>? propertiesToModify = default,
        CancellationToken cancellationToken = default)
    {
        await _messageActions.AbandonMessageAsync(message, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}