using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moosesoft.Azure.Messaging.ServiceBus;
using Moosesoft.Azure.Messaging.ServiceBus.Builders;
using SampleCore;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((builderContext, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddSingleton(
            MessageContextProcessorBuilder
                .Configure(new ServiceBusEntityDescription(builderContext.Configuration["ServiceBusQueueName"]))
                .WithMessageProcessor<SampleMessageProcessor>()
                .WithCloneMessageFailurePolicy(ex => ex is InvalidOperationException)
                //.WithExponentialDelayCalculatorStrategy()
                .WithFixedDelayCalculatorStrategy(TimeSpan.FromSeconds(10))
                //.WithLinearDelayCalculatorStrategy(TimeSpan.FromSeconds(2))
                //.WithZeroDelayCalculatorStrategy()
                .Build(ex => ex is ArgumentOutOfRangeException));
    })
    .Build();

host.Run();
