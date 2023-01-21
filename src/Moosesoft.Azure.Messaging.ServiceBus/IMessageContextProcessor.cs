using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.Messaging.ServiceBus;

/// <summary>
/// Defines an object that processes a <see cref="ServiceBusReceivedMessageContext"/>
/// </summary>
public interface IMessageContextProcessor
{
    /// <summary>
    /// Optional name for the MessageContextProcessor. By default is "default"
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Processes the received <see cref="ServiceBusReceivedMessageContext"/>
    /// </summary>
    /// <param name="messageContext">Context to be processed.</param>
    /// <param name="cancellationToken">Optional cancellation token provided to check for cancellation upstream.</param>
    /// <returns><see cref="Task"/></returns>
    Task ProcessMessageContextAsync(IServiceBusReceivedMessageContext messageContext, CancellationToken cancellationToken = default);
}