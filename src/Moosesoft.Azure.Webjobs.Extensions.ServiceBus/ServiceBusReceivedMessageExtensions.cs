namespace Moosesoft.Azure.Webjobs.Extensions.ServiceBus;

/// <summary>
/// Provides mechanism of creating instances of <see cref="MessageContext"/>.
/// </summary>
public static class ServiceBusReceivedMessageExtensions
{
    /// <summary>
    /// Creates a new instance of <see cref="MessageContext"/> from <see cref="ServiceBusReceivedMessage"/>.
    /// </summary>
    /// <param name="message">Message received from Azure Service Bus.</param>
    /// <param name="messageActions">Message actions that can be used to perform operations against the received message.</param>
    /// <param name="client">Underlying Azure Service Bus client used to process received messages.</param>
    /// <returns><see cref="MessageContext"/></returns>
    public static MessageContext CreateMessageContext(
        this ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        ServiceBusClient client)
    {
        return new WebJobsServiceBusReceivedMessageContext(message, messageActions, client);
    }
}