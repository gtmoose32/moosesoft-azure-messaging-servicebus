namespace Moosesoft.Azure.Messaging.ServiceBus;

/// <summary>
/// This class provides properties that describe an Azure Service Bus entity.
/// </summary>
public class ServiceBusEntityDescription
{
    private readonly string _entityName;

    /// <summary>
    /// Queue entity name.
    /// </summary>
    public string QueueName => IsQueue() ? _entityName : null;

    /// <summary>
    /// Topic entity name.
    /// </summary>
    public string TopicName => !IsQueue() ? _entityName : null;

    /// <summary>
    /// Subscription entity name 
    /// </summary>
    public string SubscriptionName { get; }

    /// <summary>
    /// Creates an instance that provides description of queue entities.
    /// </summary>
    /// <param name="queueName">Name of the queue.</param>
    /// <exception cref="ArgumentException"></exception>
    public ServiceBusEntityDescription(string queueName)
    {
        if (string.IsNullOrWhiteSpace(queueName))
            throw new ArgumentException("Cannot be null, empty or whitespace.", nameof(queueName));

        _entityName = queueName;
    }

    /// <summary>
    /// Creates an instance that provides description of topic and subscription entities.
    /// </summary>
    /// <param name="topicName">Name of the topic.</param>
    /// <param name="subscriptionName">Name of the subscription.</param>
    /// <exception cref="ArgumentException"></exception>
    public ServiceBusEntityDescription(string topicName, string subscriptionName)
    {
        if (string.IsNullOrWhiteSpace(topicName))
            throw new ArgumentException("Cannot be null, empty or whitespace.", nameof(topicName));

        if (string.IsNullOrWhiteSpace(subscriptionName))
            throw new ArgumentException("Cannot be null, empty or whitespace.", nameof(subscriptionName));

        _entityName = topicName;
        SubscriptionName = subscriptionName;
    }

    /// <summary>
    /// Determines whether the entity description is a queue.
    /// </summary>
    /// <returns><see cref="bool"/></returns>
    public bool IsQueue() => SubscriptionName == null;
}