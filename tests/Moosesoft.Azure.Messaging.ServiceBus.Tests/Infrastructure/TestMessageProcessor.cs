using Azure.Messaging.ServiceBus;

namespace Moosesoft.Azure.Messaging.ServiceBus.Tests.Infrastructure;

[ExcludeFromCodeCoverage]
public class TestMessageProcessor : IMessageProcessor
{
    public Task ProcessMessageAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}