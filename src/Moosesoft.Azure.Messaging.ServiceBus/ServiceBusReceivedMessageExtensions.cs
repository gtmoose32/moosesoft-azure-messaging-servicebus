﻿namespace Moosesoft.Azure.Messaging.ServiceBus;

/// <summary>
/// Provides mechanism of creating instances of <see cref="MessageContext"/>.
/// </summary>
public static class ServiceBusReceivedMessageExtensions
{
    /// <summary>
    /// Creates a new instance of <see cref="MessageContext"/> from <see cref="ServiceBusReceivedMessage"/>.
    /// </summary>
    /// <param name="message">The message from which the <see cref="MessageContext"/> is created.</param>
    /// <param name="receiver"><see cref="ServiceBusReceiver"/> used to receive the <see cref="ServiceBusReceivedMessage"/>.</param>
    /// <param name="client"><see cref="ServiceBusClient"/> used to create <see cref="ServiceBusSender"/> instances.</param>
    /// <returns><see cref="MessageContext"/></returns>
    public static MessageContext CreateMessageContext(
        this ServiceBusReceivedMessage message,
        ServiceBusReceiver receiver, 
        ServiceBusClient client)
    {
        return new ServiceBusReceivedMessageContext(message, receiver, client);
    }
}