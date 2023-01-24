using AzureFunctionSample;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moosesoft.Azure.Messaging.ServiceBus;
using Moosesoft.Azure.Messaging.ServiceBus.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

[assembly: FunctionsStartup(typeof(Startup))]
namespace AzureFunctionSample;

[ExcludeFromCodeCoverage]
public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services
            .AddSingleton(provider => 
                MessageContextProcessorBuilder
                    .Configure(new ServiceBusEntityDescription(provider.GetRequiredService<IConfiguration>()["ServiceBusQueueName"]))
                    .WithMessageProcessor<SampleMessageProcessor>()
                    .WithCloneMessageFailurePolicy(ex => ex is InvalidOperationException)
                    //.WithExponentialDelayCalculatorStrategy()
                    .WithFixedDelayCalculatorStrategy(TimeSpan.FromSeconds(10))
                    //.WithLinearDelayCalculatorStrategy(TimeSpan.FromSeconds(2))
                    //.WithZeroDelayCalculatorStrategy()
                    .Build(ex => ex is ArgumentOutOfRangeException));
    }
}