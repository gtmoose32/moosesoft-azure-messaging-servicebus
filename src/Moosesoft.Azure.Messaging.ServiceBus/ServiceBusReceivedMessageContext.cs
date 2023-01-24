using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.Messaging.ServiceBus;

/// <inheritdoc />
public class ServiceBusReceivedMessageContext : MessageContextBase
{
    private readonly ServiceBusReceiver _receiver;

    public ServiceBusReceivedMessageContext(ServiceBusReceivedMessage message, ServiceBusClient client, ServiceBusReceiver receiver)
        : base(message, client)
    {
        _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
    }


    /// <inheritdoc />
    public override async Task DeadLetterMessageAsync(string reason, CancellationToken cancellationToken)
    {
        await _receiver.DeadLetterMessageAsync(
                Message, reason, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override async Task CompleteMessageAsync(CancellationToken cancellationToken)
    {
        await _receiver.CompleteMessageAsync(Message, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override async Task AbandonMessageAsync(IDictionary<string, object> propertiesToModify = null, CancellationToken cancellationToken = default)
    {
        await _receiver.AbandonMessageAsync(Message, propertiesToModify, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}