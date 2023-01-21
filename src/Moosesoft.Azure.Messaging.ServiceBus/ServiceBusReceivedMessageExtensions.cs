using Azure.Messaging.ServiceBus;

namespace Moosesoft.Azure.Messaging.ServiceBus;

internal static class ServiceBusReceivedMessageExtensions
{
    public static int GetRetryCount(this ServiceBusReceivedMessage message) =>
        message.ApplicationProperties.TryGetValue(Constants.RetryCountKey, out var count) && count != null
            ? (int)count
            : 0;

    public static int GetDeliveryCount(this ServiceBusReceivedMessage message) => message.DeliveryCount;
}