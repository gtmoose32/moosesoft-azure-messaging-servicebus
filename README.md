# Moosesoft.Azure.Messaging.Servicebus
[![nuget](https://img.shields.io/nuget/v/Moosesoft.Azure.Messaging.ServiceBus.svg)](https://www.nuget.org/packages/Moosesoft.Azure.Messaging.ServiceBus/)
![Nuget](https://img.shields.io/nuget/dt/Moosesoft.Azure.Messaging.ServiceBus)

## What is it?
A library for .NET that uses policies to handle transient exceptions while processing messages from [Azure Service Bus](https://github.com/Azure/azure-sdk-for-net/blob/Azure.Messaging.ServiceBus_7.12.0/sdk/servicebus/Azure.Messaging.ServiceBus/README.md).  Failure policies can apply delays to further message processing based on delay calculation strategies. 

## Installing Moosesoft.Azure.Messaging.Servicebus

```
dotnet add package Moosesoft.Azure.Messaging.Servicebus
```

# Moosesoft.Azure.Webjobs.Extensions.ServiceBus
[![nuget](https://img.shields.io/nuget/v/Moosesoft.Azure.Webjobs.Extensions.ServiceBus.svg)](https://www.nuget.org/packages/Moosesoft.Azure.Webjobs.Extensions.ServiceBus/)
![Nuget](https://img.shields.io/nuget/dt/Moosesoft.Azure.Webjobs.Extensions.ServiceBus)

## What is it?

A library for .Net that provides [Azure Functions In-process Model Service Bus trigger](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-service-bus-trigger?tabs=csharp) support for handling transient exceptions by leveraging capabilities from [Moosesoft.Azure.Messaging.Servicebus](https://www.nuget.org/packages/Moosesoft.Azure.Messaging.ServiceBus).

## Installing Moosesoft.Azure.Webjobs.Extensions.ServiceBus

```
dotnet add package Moosesoft.Azure.Webjobs.Extensions.ServiceBus
```

# Moosesoft.Azure.Functions.Worker.Extensions.ServiceBus
[![nuget](https://img.shields.io/nuget/v/Moosesoft.Azure.Functions.Worker.Extensions.ServiceBus.svg)](https://www.nuget.org/packages/Moosesoft.Azure.Functions.Worker.Extensions.ServiceBus/)
![Nuget](https://img.shields.io/nuget/dt/Moosesoft.Azure.Functions.Worker.Extensions.ServiceBus)

## What is it?

A library for .Net that provides [Azure Functions Isolated Worker Model Service Bus trigger](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-service-bus-trigger?tabs=csharp) support for handling transient exceptions with failure policies that use delay calculator strategies to delay further message processing on retry attempts.

## Installing Moosesoft.Azure.Functions.Worker.Extensions.ServiceBus

```
dotnet add package Moosesoft.Azure.Functions.Worker.Extensions.ServiceBus
```

## Samples

Both Isolated Worker and In-process Models for [Azure Functions Service Bus Trigger](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-service-bus-trigger?tabs=csharp) samples can be found [here](https://github.com/gtmoose32/moosesoft-azure-messaging-servicebus/tree/master/samples/).  Note:  Azure Functions Service Bus Trigger must have auto complete messages turned off.  Otherwise, the package will fail because the Azure Functions runtime will complete or abandon the lock behind the scenes.  That setting is configured in the host.json files for both samples.

