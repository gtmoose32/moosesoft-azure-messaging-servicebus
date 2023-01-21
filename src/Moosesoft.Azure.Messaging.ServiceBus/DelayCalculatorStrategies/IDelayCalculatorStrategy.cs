using System;

namespace Moosesoft.Azure.Messaging.ServiceBus.DelayCalculatorStrategies;

/// <summary>
/// Defines a strategy for calculating delay for message processing retries.
/// </summary>
public interface IDelayCalculatorStrategy
{
    /// <summary>
    /// Performs calculation to determine how much time to wait for next retry.
    /// </summary>
    /// <param name="attempts">The number of attempts to process the message.</param>
    /// <returns>Delay that should be applied to the message before it is retried.</returns>
    TimeSpan Calculate(int attempts);
}