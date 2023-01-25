using System.Linq;

namespace Moosesoft.Azure.Messaging.ServiceBus.DelayCalculatorStrategies;

/// <summary>
/// This delay calculator strategy uses an exponential model to calculate delays.
/// </summary>
public class ExponentialDelayCalculatorStrategy : IDelayCalculatorStrategy
{
    private static readonly TimeSpan DefaultMaxDelayTime = TimeSpan.FromMinutes(60);

    private static readonly TimeSpan DefaultInitialDelay = TimeSpan.FromSeconds(100);

    private readonly TimeSpan _maxDelay;

    private readonly int _initialBackOffDelaySeconds;

    /// <summary>
    /// Initialize a new instance <see cref="ExponentialDelayCalculatorStrategy"/>.
    /// </summary>
    /// <param name="maxDelayTime"></param>
    public ExponentialDelayCalculatorStrategy(TimeSpan maxDelayTime) : this(maxDelayTime, DefaultInitialDelay)
    {

    }

    /// <summary>
    /// Initialize a new instance <see cref="ExponentialDelayCalculatorStrategy"/>.
    /// </summary>
    /// <param name="maxDelayTime">The maximum back off that would be calculated</param>
    /// <param name="initialDelay">The initial back off</param>
    public ExponentialDelayCalculatorStrategy(TimeSpan maxDelayTime, TimeSpan initialDelay)
    {
        _maxDelay = maxDelayTime > TimeSpan.Zero ? maxDelayTime : DefaultMaxDelayTime;
        _initialBackOffDelaySeconds = (int)(initialDelay > TimeSpan.Zero ? initialDelay : DefaultInitialDelay).TotalSeconds;
    }

    /// <inheritdoc cref="IDelayCalculatorStrategy"/>
    public virtual TimeSpan Calculate(int attempts)
        => new[] { TimeSpan.FromSeconds(_initialBackOffDelaySeconds * Math.Pow(attempts, 2)), _maxDelay }.Min();

    /// <summary>
    /// Creates an instance of this delay calculator strategy with default settings.
    /// </summary>
    public static readonly IDelayCalculatorStrategy Default = new ExponentialDelayCalculatorStrategy(DefaultMaxDelayTime);
}