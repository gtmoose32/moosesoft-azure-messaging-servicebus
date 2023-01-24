using Azure.Messaging.ServiceBus;
using Moosesoft.Azure.Messaging.ServiceBus.DelayCalculatorStrategies;
using Moosesoft.Azure.Messaging.ServiceBus.FailurePolicies;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Moosesoft.Azure.Messaging.ServiceBus.Builders;

public class MessageContextProcessorBuilder 
    : IMessageProcessorHolder, IFailurePolicyHolder, IDelayCalculatorStrategyHolder, IMessageContextProcessorBuilder
{
    private readonly BuilderState _state;

    internal MessageContextProcessorBuilder(ServiceBusEntityDescription serviceBusEntityDescription)
    {
        _state = new BuilderState(serviceBusEntityDescription);
    }

    public static IMessageProcessorHolder Configure(ServiceBusEntityDescription serviceBusEntityDescription)
        => new MessageContextProcessorBuilder(serviceBusEntityDescription);

    public IFailurePolicyHolder WithMessageProcessor<TProcessor>(TProcessor messageProcessor) where TProcessor : IMessageProcessor
    {
        if (messageProcessor == null) throw new ArgumentNullException(nameof(messageProcessor));

        _state.MessageProcessor = messageProcessor;
        return this;
    }

    public IFailurePolicyHolder WithMessageProcessor<TProcessor>() where TProcessor : IMessageProcessor, new()
    {
        return WithMessageProcessor(new TProcessor());
    }

    public IFailurePolicyHolder WithMessageProcessor(Func<ServiceBusReceivedMessage, CancellationToken, Task> processMessage)
    {
        return WithMessageProcessor(new FuncMessageProcessor(processMessage));
    }

    public IDelayCalculatorStrategyHolder WithCloneMessageFailurePolicy(Func<Exception, bool> canHandle = null, int maxMessageDeliveryCount = 10)
    {
        canHandle ??= DefaultCanHandle;
        _state.FailurePolicyFactory = () => new CloneMessageFailurePolicy(canHandle, _state.DelayCalculatorStrategy, maxMessageDeliveryCount);
        return this;
    }

    public IMessageContextProcessorBuilder WithAbandonMessageFailurePolicy()
    {
        return WithFailurePolicy(new AbandonMessageFailurePolicy());
    }

    public IMessageContextProcessorBuilder WithFailurePolicy<TFailurePolicy>(TFailurePolicy failurePolicy) 
        where TFailurePolicy : IFailurePolicy
    {
        if (failurePolicy == null) throw new ArgumentNullException(nameof(failurePolicy));

        _state.FailurePolicyFactory = () => failurePolicy;
        return this;
    }
    
    public IMessageContextProcessorBuilder WithDelayCalculatorStrategy<TStrategy>(TStrategy delayCalculatorStrategy) 
        where TStrategy : IDelayCalculatorStrategy
    {
        if (delayCalculatorStrategy == null) throw new ArgumentNullException(nameof(delayCalculatorStrategy));

        _state.DelayCalculatorStrategy = delayCalculatorStrategy;
        return this;
    }

    public IMessageContextProcessorBuilder WithDelayCalculatorStrategy<TStrategy>() 
        where TStrategy : IDelayCalculatorStrategy, new()
    {
        return WithDelayCalculatorStrategy(new TStrategy());
    }

    public IMessageContextProcessorBuilder WithExponentialDelayCalculatorStrategy(TimeSpan maxDelay = default)
    {
        var strategy = maxDelay == TimeSpan.Zero
            ? ExponentialDelayCalculatorStrategy.Default
            : new ExponentialDelayCalculatorStrategy(maxDelay);

        return WithDelayCalculatorStrategy(strategy);
    }

    public IMessageContextProcessorBuilder WithFixedDelayCalculatorStrategy(TimeSpan delayTime = default)
    {
        var strategy = delayTime == TimeSpan.Zero
            ? FixedDelayCalculatorStrategy.Default
            : new FixedDelayCalculatorStrategy(delayTime);

        return WithDelayCalculatorStrategy(strategy);
    }

    public IMessageContextProcessorBuilder WithLinearDelayCalculatorStrategy(TimeSpan delayTime = default)
    {
        var strategy = delayTime == TimeSpan.Zero
            ? LinearDelayCalculatorStrategy.Default
            : new LinearDelayCalculatorStrategy(delayTime);

        return WithDelayCalculatorStrategy(strategy);
    }

    public IMessageContextProcessorBuilder WithZeroDelayCalculatorStrategy()
    {
        return WithDelayCalculatorStrategy(new ZeroDelayCalculatorStrategy());
    }

    public IMessageContextProcessor Build(Func<Exception, bool> shouldCompleteOn = null, string name = "default")
    {
        return new ServiceBusReceivedMessageContextProcessor(
            _state.EntityDescription, 
            _state.MessageProcessor,
            _state.FailurePolicyFactory(), 
            shouldCompleteOn, 
            name);
    }

    internal BuilderState GetBuilderState() => _state;

    internal static Func<Exception, bool> DefaultCanHandle => _ => true;
}
