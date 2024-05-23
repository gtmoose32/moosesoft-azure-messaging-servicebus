using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Moosesoft.Azure.Functions.Worker.Extensions.ServiceBus;
using Moosesoft.Azure.Messaging.ServiceBus;
using System.Diagnostics.CodeAnalysis;

namespace IsolatedWorkerAzureFunctionSample;

[ExcludeFromCodeCoverage]
public class IsolatedWorkerFunction
{
    private readonly IMessageContextProcessor _messageContextProcessor;

    public IsolatedWorkerFunction(IMessageContextProcessor messageContextProcessor)
    {
        _messageContextProcessor = messageContextProcessor;
    }

    [Function(nameof(IsolatedWorkerFunction))]
    public async Task Run(
        [ServiceBusTrigger("%ServiceBusQueueName%", Connection = "ServiceBusConnectionString")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        ServiceBusClient client)
    {
        Console.WriteLine($"C# ServiceBus queue trigger function processed message: {message.MessageId}");

        var messageContext = message.CreateMessageContext(messageActions, client);
        await _messageContextProcessor.ProcessMessageContextAsync(messageContext).ConfigureAwait(false);
    }
}