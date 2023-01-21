using Moosesoft.Azure.Messaging.ServiceBus.FailurePolicies;
using System;

namespace Moosesoft.Azure.Messaging.ServiceBus.Abstractions.Builder;

/// <summary>
/// Provides a holding mechanism for instances of <see cref="IFailurePolicy"/> to help constructing message pumps.
/// </summary>
public interface IFailurePolicyHolder
{
    /// <summary>
    /// Sets the instance of <see cref="IFailurePolicy"/> to <see cref="CloneMessageFailurePolicy"/> for the message pump builder to use.
    /// </summary>
    /// <param name="canHandle">Function providing <see cref="bool"/> for which exceptions should be handled by the <see cref="IFailurePolicy"/>.</param>
    /// <returns><see cref="IDelayCalculatorStrategyHolder"/></returns>
    IDelayCalculatorStrategyHolder WithCloneMessageFailurePolicy(Func<Exception, bool> canHandle = null, int maxMessageDeliveryCount = 10);


    /// <summary>
    /// Sets the instance of <see cref="IFailurePolicy"/> to <see cref="AbandonMessageFailurePolicy"/> for the message pump builder to use.
    /// </summary>
    /// <returns>A message pump builder</returns>
    IMessageContextProcessorBuilder WithAbandonMessageFailurePolicy();

    /// <summary>
    /// Sets the instance of <see cref="IFailurePolicy"/> for the message pump builder to use.
    /// </summary>
    /// <typeparam name="TFailurePolicy">Type of <see cref="IFailurePolicy"/></typeparam>
    /// <param name="failurePolicy"><see cref="IFailurePolicy"/> to use for the use for the message pump builder.</param>
    /// <returns>A message pump builder</returns>
    IMessageContextProcessorBuilder WithFailurePolicy<TFailurePolicy>(TFailurePolicy failurePolicy)
        where TFailurePolicy : IFailurePolicy;
}