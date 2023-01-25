namespace Moosesoft.Azure.Messaging.ServiceBus.DelayCalculatorStrategies;

/// <summary>
/// This strategy provides a constant delay for every Calculate call regardless of attempt count.
/// </summary>
public class FixedDelayCalculatorStrategy : IDelayCalculatorStrategy
{
    private static readonly TimeSpan DefaultDelayTimeSpan = TimeSpan.FromMinutes(5);

    private readonly TimeSpan _delay;

    /// <summary>
    /// Initialize a new instance <see cref="FixedDelayCalculatorStrategy"/>.
    /// </summary>
    /// <param name="delayTimeSpan">Fixed TimeSpan returned always when this strategy is asked to calculate new delay.</param>
    public FixedDelayCalculatorStrategy(TimeSpan delayTimeSpan)
    {
        _delay = delayTimeSpan >= TimeSpan.Zero ? delayTimeSpan : DefaultDelayTimeSpan;
    }

    /// <inheritdoc cref="IDelayCalculatorStrategy"/>
    public TimeSpan Calculate(int attempts) => _delay;

    /// <summary>
    /// Creates an instance of this delay calculator strategy with default settings.
    /// </summary>
    public static IDelayCalculatorStrategy Default => new FixedDelayCalculatorStrategy(DefaultDelayTimeSpan);
}