using System;

namespace Moosesoft.Azure.Messaging.ServiceBus;

public class ServiceBusEntityDescription
{
    private readonly string _entityName;

    public string QueueName => IsQueue() ? _entityName : null;

    public string TopicName => !IsQueue() ? _entityName : null;

    public string SubscriptionName { get; }

    public ServiceBusEntityDescription(string queueName)
    {
        if (string.IsNullOrWhiteSpace(queueName))
            throw new ArgumentException("Cannot be null, empty or whitespace.", nameof(queueName));

        _entityName = queueName;
    }

    public ServiceBusEntityDescription(string topicName, string subscriptionName)
    {
        if (string.IsNullOrWhiteSpace(topicName))
            throw new ArgumentException("Cannot be null, empty or whitespace.", nameof(topicName));

        if (string.IsNullOrWhiteSpace(subscriptionName))
            throw new ArgumentException("Cannot be null, empty or whitespace.", nameof(subscriptionName));

        _entityName = topicName;
        SubscriptionName = subscriptionName;
    }

    public bool IsQueue() => SubscriptionName == null;
}