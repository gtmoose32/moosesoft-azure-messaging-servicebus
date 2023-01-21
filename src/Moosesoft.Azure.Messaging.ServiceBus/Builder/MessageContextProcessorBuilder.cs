using Azure.Messaging.ServiceBus;
using Moosesoft.Azure.Messaging.ServiceBus.Builder;
using Moosesoft.Azure.Messaging.ServiceBus.DelayCalculatorStrategies;
using Moosesoft.Azure.Messaging.ServiceBus.FailurePolicies;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.Messaging.ServiceBus.Abstractions.Builder;

public class MessageContextProcessorBuilder 
    : IMessageProcessorHolder, IFailurePolicyHolder, IDelayCalculatorStrategyHolder, IMessageContextProcessorBuilder
{
    private readonly ServiceBusEntityDescription _serviceBusEntityDescription;
    private readonly BuilderState _state;

    private MessageContextProcessorBuilder(ServiceBusEntityDescription serviceBusEntityDescription)
    {
        _serviceBusEntityDescription = serviceBusEntityDescription ?? throw new ArgumentNullException(nameof(serviceBusEntityDescription));
        _state = new BuilderState();
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

    public IMessageContextProcessorBuilder WithExponentialDelayCalculatorStrategy()
    {
        return WithDelayCalculatorStrategy(ExponentialDelayCalculatorStrategy.Default);
    }

    public IMessageContextProcessorBuilder WithExponentialDelayCalculatorStrategy(TimeSpan maxDelay)
    {
        return WithDelayCalculatorStrategy(new ExponentialDelayCalculatorStrategy(maxDelay));
    }

    public IMessageContextProcessorBuilder WithFixedDelayCalculatorStrategy()
    {
        return WithDelayCalculatorStrategy(FixedDelayCalculatorStrategy.Default);
    }

    public IMessageContextProcessorBuilder WithFixedDelayCalculatorStrategy(TimeSpan delayTime)
    {
        return WithDelayCalculatorStrategy(new FixedDelayCalculatorStrategy(delayTime));
    }

    public IMessageContextProcessorBuilder WithLinearDelayCalculatorStrategy()
    {
        return WithDelayCalculatorStrategy(LinearDelayCalculatorStrategy.Default);
    }

    public IMessageContextProcessorBuilder WithLinearDelayCalculatorStrategy(TimeSpan delayTime)
    {
        return WithDelayCalculatorStrategy(new LinearDelayCalculatorStrategy(delayTime));
    }

    public IMessageContextProcessorBuilder WithZeroDelayCalculatorStrategy()
    {
        return WithDelayCalculatorStrategy(new ZeroDelayCalculatorStrategy());
    }

    public IMessageContextProcessor Build(Func<Exception, bool> shouldCompleteOn = null)
    {
        return new ServiceBusReceivedMessageContextProcessor(
            _serviceBusEntityDescription, 
            _state.MessageProcessor,
            _state.FailurePolicyFactory(), 
            shouldCompleteOn);
    }

    public IMessageContextProcessor Build(string name, Func<Exception, bool> shouldCompleteOn = null)
    {
        return new ServiceBusReceivedMessageContextProcessor(
            _serviceBusEntityDescription,
            _state.MessageProcessor,
            _state.FailurePolicyFactory(),
            shouldCompleteOn, 
            name);
    }
}

class MyClass
{

    void foo()
    {
        MessageContextProcessorBuilder
            .Configure(new ServiceBusEntityDescription("queue"))
            .WithMessageProcessor((message, token) => Task.CompletedTask)
            .WithCloneMessageFailurePolicy(ex => ex is InvalidOperationException)
            .WithFixedDelayCalculatorStrategy(TimeSpan.FromMinutes(10))
            .Build("ProcessorName", ex => ex is NotSupportedException);
            
    }
}