namespace Moosesoft.Azure.Messaging.ServiceBus.Builders;

/// <summary>
/// Defines a holding mechanism for instances of <see cref="IDelayCalculatorStrategy"/> to help build instances of <see cref="IMessageContextProcessor"/>.
/// </summary>
public interface IDelayCalculatorStrategyHolder
{
    /// <summary>
    /// Sets the instance of <see cref="IDelayCalculatorStrategy"/> for the message pump builder to use.
    /// </summary>
    /// <typeparam name="TStrategy">Type of <see cref="IDelayCalculatorStrategy"/></typeparam>
    /// <param name="delayCalculatorStrategy"><see cref="IDelayCalculatorStrategy"/> to use for the use for the message pump builder.</param>
    /// <returns><see cref="IMessageContextProcessorBuilder"/></returns>
    IMessageContextProcessorBuilder WithDelayCalculatorStrategy<TStrategy>(TStrategy delayCalculatorStrategy)
        where TStrategy : IDelayCalculatorStrategy;

    /// <summary>
    /// Sets the instance of <see cref="IDelayCalculatorStrategy"/> for the message pump builder to use.
    /// </summary>
    /// <returns><see cref="IMessageContextProcessorBuilder"/></returns>
    IMessageContextProcessorBuilder WithDelayCalculatorStrategy<TStrategy>()
        where TStrategy : IDelayCalculatorStrategy, new();

    /// <summary>
    /// Sets the instance of <see cref="IDelayCalculatorStrategy"/> to <see cref="ExponentialDelayCalculatorStrategy"/> for the message pump builder to use.
    /// </summary>
    /// <param name="maxDelay">Maximum amount of time this strategy will return.</param>
    /// <returns><see cref="IMessageContextProcessorBuilder"/></returns>
    IMessageContextProcessorBuilder WithExponentialDelayCalculatorStrategy(TimeSpan maxDelay = default);

    /// <summary>
    /// Sets the instance of <see cref="IDelayCalculatorStrategy"/> to <see cref="FixedDelayCalculatorStrategy"/> for the message pump builder to use.
    /// </summary>
    /// <param name="delayTime">Fixed amount of time which will be returned.</param>
    /// <returns><see cref="IMessageContextProcessorBuilder"/></returns>
    IMessageContextProcessorBuilder WithFixedDelayCalculatorStrategy(TimeSpan delayTime = default);

    /// <summary>
    /// Sets the instance of <see cref="IDelayCalculatorStrategy"/> to <see cref="LinearDelayCalculatorStrategy"/> for the message pump builder to use.
    /// </summary>
    /// <param name="delayTime">Amount of time to use with attempt multiplier.</param>
    /// <returns><see cref="IMessageContextProcessorBuilder"/></returns>
    IMessageContextProcessorBuilder WithLinearDelayCalculatorStrategy(TimeSpan delayTime = default);

    /// <summary>
    /// Sets the instance of <see cref="IDelayCalculatorStrategy"/> to <see cref="ZeroDelayCalculatorStrategy"/> for the message pump builder to use.
    /// </summary>
    /// <returns>A message pump builder.</returns>
    IMessageContextProcessorBuilder WithZeroDelayCalculatorStrategy();
}