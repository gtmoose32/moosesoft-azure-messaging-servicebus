using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs.ServiceBus;
using Moosesoft.Azure.Messaging.ServiceBus;

namespace Moosesoft.Azure.Webjobs.Extensions.ServiceBus;

/// <summary>
/// <see cref="IServiceBusReceivedMessageContext"/> implementation compatible for use with Microsoft.Azure.WebJobs and AzureFunctions
/// which exposes <see cref="ServiceBusMessageActions"/> for handling messages.
/// </summary>
public class WebJobsServiceBusReceivedMessageContext : ServiceBusReceivedMessageContextBase
{
    private readonly ServiceBusMessageActions _messageActions;

    /// <summary>
    /// Creates a new <see cref="WebJobsServiceBusReceivedMessageContext"/>
    /// </summary>
    /// <param name="message"></param>
    /// <param name="messageActions"></param>
    /// <param name="client"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public WebJobsServiceBusReceivedMessageContext(ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions, ServiceBusClient client)
        : base(message, client)
    {
        _messageActions = messageActions ?? throw new ArgumentNullException(nameof(messageActions));
    }

    /// <inheritdoc />
    public override async Task DeadLetterMessageAsync(string reason, CancellationToken cancellationToken)
    {
        await _messageActions.DeadLetterMessageAsync(Message, reason, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override async Task CompleteMessageAsync(CancellationToken cancellationToken)
    {
        await _messageActions.CompleteMessageAsync(Message, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override async Task AbandonMessageAsync(IDictionary<string, object>? propertiesToModify = default, CancellationToken cancellationToken = default)
    {
        await _messageActions.AbandonMessageAsync(Message, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}