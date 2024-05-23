using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Moosesoft.Azure.Messaging.ServiceBus;
using Moosesoft.Azure.Webjobs.Extensions.ServiceBus;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace InProcessAzureFunctionSample;

[ExcludeFromCodeCoverage]
public class InProcessFunction
{
    private readonly IMessageContextProcessor _messageContextProcessor;

    public InProcessFunction(IMessageContextProcessor messageContextProcessor)
    {
        _messageContextProcessor = messageContextProcessor ?? throw new ArgumentNullException(nameof(messageContextProcessor));
    }

    [FunctionName("SampleFunction")]
    public async Task ProcessMessageAsync(
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