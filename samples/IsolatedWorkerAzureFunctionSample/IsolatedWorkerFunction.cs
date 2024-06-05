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
    private readonly ServiceBusClient _serviceBusClient;

    public IsolatedWorkerFunction(IMessageContextProcessor messageContextProcessor, ServiceBusClient serviceBusClient)
    {
        _messageContextProcessor = messageContextProcessor;
        _serviceBusClient = serviceBusClient;
    }

    [Function(nameof(IsolatedWorkerFunction))]
    public async Task Run(
        [ServiceBusTrigger("%ServiceBusQueueName%", Connection = "ServiceBusConnectionString")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        Console.WriteLine($"C# ServiceBus queue trigger function processed message: {message.MessageId}");

        var messageContext = message.CreateMessageContext(messageActions, _serviceBusClient);
        await _messageContextProcessor.ProcessMessageContextAsync(messageContext).ConfigureAwait(false);
    }
}