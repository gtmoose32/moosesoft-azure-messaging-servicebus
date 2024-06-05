﻿namespace Moosesoft.Azure.Webjobs.Extensions.ServiceBus;

/// <summary>
/// <see cref="MessageContext"/> implementation for use with AzureFunctions In-process Model Service Bus Trigger.
/// </summary>
internal class WebJobsServiceBusReceivedMessageContext : MessageContext
{
    private readonly ServiceBusMessageActions _messageActions;

    /// <summary>
    /// Creates a new <see cref="WebJobsServiceBusReceivedMessageContext"/>
    /// </summary>
    /// <param name="message">Message received from Azure Service Bus.</param>
    /// <param name="messageActions">Message actions that can be used to perform operations against the received message.</param>
    /// <param name="client">Underlying Azure Service Bus client used to process received messages.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public WebJobsServiceBusReceivedMessageContext(ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions, ServiceBusClient client)
        : base(message, client)
    {
        _messageActions = messageActions;
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
        await _messageActions.AbandonMessageAsync(Message, propertiesToModify, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}