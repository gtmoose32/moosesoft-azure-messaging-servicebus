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
    /// <param name="reason"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeadLetterMessageAsync(string reason, CancellationToken cancellationToken);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CompleteMessageAsync(CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertiesToModify"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AbandonMessageAsync(IDictionary<string, object> propertiesToModify = null, CancellationToken cancellationToken = default);
}