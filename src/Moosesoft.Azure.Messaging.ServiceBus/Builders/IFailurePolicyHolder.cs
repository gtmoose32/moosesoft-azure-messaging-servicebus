namespace Moosesoft.Azure.Messaging.ServiceBus.Builders;

/// <summary>
/// Defines a holding mechanism for instances of <see cref="IFailurePolicy"/> to help build instances of <see cref="IMessageContextProcessor"/>.
/// </summary>
public interface IFailurePolicyHolder
{
    /// <summary>
    /// Sets the instance of <see cref="IFailurePolicy"/> to <see cref="CloneMessageFailurePolicy"/> for the message pump builder to use.
    /// </summary>
    /// <param name="canHandle">Function providing <see cref="bool"/> for which exceptions should be handled by the <see cref="IFailurePolicy"/>.</param>
    /// <param name="maxMessageDeliveryCount">Maximum times the message can delivered before failure policy will move it to the DLQ.</param>
    /// <returns><see cref="IDelayCalculatorStrategyHolder"/></returns>
    IDelayCalculatorStrategyHolder WithCloneMessageFailurePolicy(Func<Exception, bool> canHandle = null, int maxMessageDeliveryCount = 10);


    /// <summary>
    /// Sets the instance of <see cref="IFailurePolicy"/> to <see cref="AbandonMessageFailurePolicy"/> for the message pump builder to use.
    /// </summary>
    /// <returns><see cref="IDelayCalculatorStrategyHolder"/></returns>
    IMessageContextProcessorBuilder WithAbandonMessageFailurePolicy();

    /// <summary>
    /// Sets the instance of <see cref="IFailurePolicy"/> for the message pump builder to use.
    /// </summary>
    /// <typeparam name="TFailurePolicy">Type of <see cref="IFailurePolicy"/></typeparam>
    /// <param name="failurePolicy"><see cref="IFailurePolicy"/> to use for the use for the message pump builder.</param>
    /// <returns><see cref="IDelayCalculatorStrategyHolder"/></returns>
    IMessageContextProcessorBuilder WithFailurePolicy<TFailurePolicy>(TFailurePolicy failurePolicy)
        where TFailurePolicy : IFailurePolicy;
}