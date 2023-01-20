using Azure.Messaging.ServiceBus;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.Messaging.ServiceBus;

/// <summary>
/// Defines an object that processes a Service Bus <see cref="ServiceBusReceivedMessage"/>
/// </summary>
public interface IMessageProcessor
{
    /// <summary>
    /// Processes a Service Bus <see cref="ServiceBusReceivedMessage"/>
    /// </summary>
    /// <param name="message">Message received from a Service Bus entity for processing.</param>
    /// <param name="cancellationToken">Cancellation token used to check for cancellation upstream.</param>
    /// <returns><see cref="Task"/></returns>
    Task ProcessMessageAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken);
}