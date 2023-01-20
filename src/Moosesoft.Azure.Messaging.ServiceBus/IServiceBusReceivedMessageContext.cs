using Azure.Messaging.ServiceBus;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Moosesoft.Azure.Messaging.ServiceBus;

/// <summary>
/// 
/// </summary>
public interface IServiceBusReceivedMessageContext
{
    /// <summary>
    /// 
    /// </summary>
    public ServiceBusReceivedMessage Message { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    ServiceBusSender CreateMessageSender(ServiceBusEntityDescription description);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="reason"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeadLetterMessageAsync(ServiceBusReceivedMessage message, string reason, CancellationToken cancellationToken);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CompleteMessageAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="propertiesToModify"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AbandonMessageAsync(ServiceBusReceivedMessage message, IDictionary<string, object> propertiesToModify = null, CancellationToken cancellationToken = default);
}