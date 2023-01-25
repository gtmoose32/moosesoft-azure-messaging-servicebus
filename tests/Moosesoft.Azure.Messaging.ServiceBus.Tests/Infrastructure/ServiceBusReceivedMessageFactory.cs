using Azure.Core.Amqp;
using System.Reflection;

namespace Moosesoft.Azure.Messaging.ServiceBus.Tests.Infrastructure;

[ExcludeFromCodeCoverage]
public static class ServiceBusReceivedMessageFactory
{
    public static ServiceBusReceivedMessage Create()
    {
        var ctor = typeof(ServiceBusReceivedMessage).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, new[] { typeof(AmqpAnnotatedMessage) });
        var message = new AmqpAnnotatedMessage(new AmqpMessageBody(new List<ReadOnlyMemory<byte>>()))
        {
            Header = { DeliveryCount = 1 },
            Properties = { MessageId = new AmqpMessageId(Guid.NewGuid().ToString()) }
        };

        return ctor?.Invoke(parameters: new object[] { message }) as ServiceBusReceivedMessage;
    }
}