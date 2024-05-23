using Azure.Messaging.ServiceBus;
using Moosesoft.Azure.Messaging.ServiceBus;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace SampleCore;

/// <summary>
/// Implementations of <see cref="IMessageProcessor"/> should process messages received from Service Bus.
/// This sample class is merely to demonstrate failure policies with back off delays.
/// </summary>
[ExcludeFromCodeCoverage]
public class SampleMessageProcessor : IMessageProcessor
{
    private readonly Random _random = new();

    public Task ProcessMessageAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken)
    {
        var num = _random.Next(0, 10);
        if (num == 0) // throw an out of range exception to demonstrate should complete on.
        {
            Console.WriteLine($"Message: {message.MessageId} completed due to exception matching should complete expression.");
            throw new ArgumentOutOfRangeException(nameof(num));
        }

        //throw exception to demonstrate failure policy on even numbers
        if (num % 2 != 0)
        {
            Console.WriteLine($"Message: {message.MessageId} re-sent due to exception matching failure policy expression.");
            throw new InvalidOperationException("Test failure policy with back off delay strategy.");
        }

        Console.WriteLine($"Message: {message.MessageId} completed since it was neither odd or zero.");
        return Task.CompletedTask; //complete on odd non-zero numbers
    }
}