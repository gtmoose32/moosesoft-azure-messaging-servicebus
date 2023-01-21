using Moosesoft.Azure.Messaging.ServiceBus.FailurePolicies;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.Messaging.ServiceBus;

/// <inheritdoc />
internal class ServiceBusReceivedMessageContextProcessor : IMessageContextProcessor
{
    private readonly IFailurePolicy _failurePolicy;
    private readonly ServiceBusEntityDescription _description;
    private readonly IMessageProcessor _messageProcessor;
    private readonly Func<Exception, bool> _shouldComplete;

    /// <inheritdoc />
    public string Name { get; }

    public ServiceBusReceivedMessageContextProcessor(
        ServiceBusEntityDescription description,
        IMessageProcessor messageProcessor,
        IFailurePolicy failurePolicy = null,
        Func<Exception, bool> shouldComplete = null,
        string name = "default")
    {
        _description = description ?? throw new ArgumentNullException(nameof(description));
        _messageProcessor = messageProcessor ?? throw new ArgumentNullException(nameof(messageProcessor));
        _failurePolicy = failurePolicy ?? new AbandonMessageFailurePolicy();
        _shouldComplete = shouldComplete;
        Name = name;

        _failurePolicy.SetEntityDescription(description);
    }

    /// <inheritdoc />
    public async Task ProcessMessageContextAsync(IServiceBusReceivedMessageContext messageContext, CancellationToken cancellationToken = default)
    {
        try
        {
            await _messageProcessor.ProcessMessageAsync(messageContext.Message, cancellationToken).ConfigureAwait(false);


            await messageContext
                .CompleteMessageAsync(messageContext.Message, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            if (await TryCompleteOnExceptionAsync(messageContext, exception, cancellationToken) ||
                await TryAbandonOnExceptionAsync(messageContext, exception, cancellationToken))
            {
                return;
            }

            await _failurePolicy.HandleFailureAsync(messageContext, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task<bool> TryCompleteOnExceptionAsync(IServiceBusReceivedMessageContext messageContext, Exception exception, CancellationToken cancellationToken)
    {
        if (_shouldComplete == null || !_shouldComplete(exception)) return false;

        await messageContext
            .CompleteMessageAsync(messageContext.Message, cancellationToken)
            .ConfigureAwait(false);

        return true;
    }

    private async Task<bool> TryAbandonOnExceptionAsync(IServiceBusReceivedMessageContext messageContext, Exception exception, CancellationToken cancellationToken)
    {
        if (_failurePolicy.CanHandle(exception)) return false;

        await messageContext
            .AbandonMessageAsync(messageContext.Message, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return true;
    }
}