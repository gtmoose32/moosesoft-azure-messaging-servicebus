using System;

namespace Moosesoft.Azure.Messaging.ServiceBus.DelayCalculatorStrategies;

/// <summary>
/// This strategy will always calculate a zero delay.
/// </summary>
public class ZeroDelayCalculatorStrategy : FixedDelayCalculatorStrategy
{
    /// <summary>
    /// Creates a new instance of this delay calculator strategy.
    /// </summary>
    public ZeroDelayCalculatorStrategy() : base(TimeSpan.Zero)
    {
    }
}