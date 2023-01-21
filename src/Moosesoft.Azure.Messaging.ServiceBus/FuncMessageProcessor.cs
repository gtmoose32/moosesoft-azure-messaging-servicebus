using Azure.Messaging.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.Messaging.ServiceBus;

internal class FuncMessageProcessor : IMessageProcessor
{
    private readonly Func<ServiceBusReceivedMessage, CancellationToken, Task> _processMessage;

    public FuncMessageProcessor(Func<ServiceBusReceivedMessage, CancellationToken, Task> processMessage)
    {
        _processMessage = processMessage ?? throw new ArgumentNullException(nameof(processMessage));
    }

    public Task ProcessMessageAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken) 
        => _processMessage(message, cancellationToken);

}