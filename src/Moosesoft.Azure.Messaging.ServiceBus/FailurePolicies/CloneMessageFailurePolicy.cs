using Azure.Messaging.ServiceBus;
using Moosesoft.Azure.Messaging.ServiceBus.DelayCalculatorStrategies;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Moosesoft.Azure.Messaging.ServiceBus.FailurePolicies;

/// <summary>
/// This failure policy will create a clone of the Service Bus Message attempting to be processed.
/// The clone will then be sent back to Service Bus while the original message is completed in an atomic transaction with the send.
/// New messages sent to Service Bus will be delayed based upon the <see cref="IDelayCalculatorStrategy"/> chosen.
/// </summary>
public class CloneMessageFailurePolicy : FailurePolicyBase
{
    /// <inheritdoc />
    public CloneMessageFailurePolicy(
        Func<Exception, bool> canHandle,
        IDelayCalculatorStrategy delayCalculatorStrategy = null,
        int maxDeliveryCount = 10)
        : base(canHandle, delayCalculatorStrategy, maxDeliveryCount)
    {
    }

    /// <inheritdoc />
    public override async Task HandleFailureAsync(MessageContextBase messageContext, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var deliveryCount = GetDeliveryCount(messageContext);
        if (deliveryCount >= MaxDeliveryCount)
        {
            await messageContext
                .DeadLetterMessageAsync($"Max delivery count of {MaxDeliveryCount} has been reached.", cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return;
        }

        var sender = messageContext.CreateMessageSender(ServiceBusEntityDescription);
        try
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            await sender.SendMessageAsync(CreateMessageToSend(messageContext, deliveryCount), cancellationToken).ConfigureAwait(false);
            await messageContext.CompleteMessageAsync(cancellationToken).ConfigureAwait(false);

            scope.Complete();
        }
        finally
        {
            await sender.CloseAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    protected override int GetDeliveryCount(MessageContextBase message) => base.GetDeliveryCount(message) + message.RetryCount;

    private ServiceBusMessage CreateMessageToSend(MessageContextBase messageContext, int deliveryCount)
    {
        var clone = messageContext.ToServiceBusMessage();
        clone.ScheduledEnqueueTime = DateTime.UtcNow + DelayCalculatorStrategy.Calculate(deliveryCount);
        clone.ApplicationProperties[Constants.RetryCountPropertyName] = deliveryCount;

        return clone;
    }
}