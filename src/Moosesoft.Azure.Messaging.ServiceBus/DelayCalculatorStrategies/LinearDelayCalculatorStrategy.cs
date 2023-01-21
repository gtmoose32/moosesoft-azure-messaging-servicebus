using System;

namespace Moosesoft.Azure.Messaging.ServiceBus.DelayCalculatorStrategies;

/// <summary>
/// This strategy performs back off delay calculations using a linear model.
/// </summary>
public class LinearDelayCalculatorStrategy : IDelayCalculatorStrategy
{
    private readonly TimeSpan _initialDelay;

    /// <summary>
    /// Initialize a new instance <see cref="LinearDelayCalculatorStrategy"/>.
    /// </summary>
    /// <param name="initialDelay"></param>
    public LinearDelayCalculatorStrategy(TimeSpan initialDelay)
    {
        _initialDelay = initialDelay;
    }

    /// <inheritdoc cref="IDelayCalculatorStrategy"/>
    public virtual TimeSpan Calculate(int attempts) => TimeSpan.FromSeconds(_initialDelay.TotalSeconds * attempts);

    /// <summary>
    /// Creates an instance of this delay calculator strategy with default settings.
    /// </summary>
    public static IDelayCalculatorStrategy Default => new LinearDelayCalculatorStrategy(TimeSpan.FromMinutes(1));
}