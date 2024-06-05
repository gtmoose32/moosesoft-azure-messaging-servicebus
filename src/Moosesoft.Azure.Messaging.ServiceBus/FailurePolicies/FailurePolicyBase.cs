namespace Moosesoft.Azure.Messaging.ServiceBus.FailurePolicies;

/// <inheritdoc cref="IFailurePolicy"/>>
public abstract class FailurePolicyBase : IFailurePolicy
{
    /// <summary>
    /// Maximum delivery count allowed
    /// </summary>
    protected int MaxDeliveryCount { get; }

    /// <summary>
    /// Back off delay calculator strategy
    /// </summary>
    protected IDelayCalculatorStrategy DelayCalculatorStrategy { get; }

    /// <summary>
    /// Failure policy entity description.
    /// </summary>
    protected ServiceBusEntityDescription ServiceBusEntityDescription { get; private set; }

    private readonly Func<Exception, bool> _canHandle;
    
    /// <summary>
    /// Initialize a new instance <see cref="IFailurePolicy"/>.
    /// </summary>
    /// <param name="canHandle">Function that determines if the exception that has occurred <see cref="IFailurePolicy"/> will be applied to it.</param>
    /// <param name="delayCalculatorStrategy"><see cref="IDelayCalculatorStrategy"/> to use when <see cref="IFailurePolicy"/> is applied.</param>
    /// <param name="maxDeliveryCount">Maximum number of message delivery counts from Azure Service Bus.</param>
    protected FailurePolicyBase(Func<Exception, bool> canHandle, IDelayCalculatorStrategy delayCalculatorStrategy = null, int maxDeliveryCount = 10)
    {
        _canHandle = canHandle ?? throw new ArgumentNullException(nameof(canHandle));
        DelayCalculatorStrategy = delayCalculatorStrategy ?? new ZeroDelayCalculatorStrategy();
        MaxDeliveryCount = maxDeliveryCount >= 0 ? maxDeliveryCount : 10;
    }

    /// <inheritdoc />
    public bool CanHandle(Exception exception) => _canHandle(exception);

    /// <inheritdoc />
    public abstract Task HandleFailureAsync(MessageContext messageContext, CancellationToken cancellationToken);

    /// <inheritdoc />
    public void SetEntityDescription(ServiceBusEntityDescription description)
    {
        ServiceBusEntityDescription = description ?? throw new ArgumentNullException(nameof(description));
    }

    /// <summary>
    /// Gets the delivery count for the specified Service Bus <see cref="ServiceBusReceivedMessage"/>.
    /// </summary>
    /// <param name="messageContext"><see cref="MessageContext"/> used to determine the delivery count.</param>
    /// <returns>Number of times the message has been delivered.</returns>
    protected virtual int GetDeliveryCount(MessageContext messageContext) => messageContext.DeliveryCount;
}