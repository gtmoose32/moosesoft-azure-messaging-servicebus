# Moosesoft.Azure.Messaging.Servicebus
[![nuget](https://img.shields.io/nuget/v/Moosesoft.Azure.Messaging.ServiceBus.svg)](https://www.nuget.org/packages/Moosesoft.Azure.Messaging.ServiceBus/)
![Nuget](https://img.shields.io/nuget/dt/Moosesoft.Azure.Messaging.ServiceBus)

## What is it?
A library for .NET that uses policies to handle transient exceptions while processing messages from [Azure Service Bus](https://github.com/Azure/azure-sdk-for-net/blob/Azure.Messaging.ServiceBus_7.12.0/sdk/servicebus/Azure.Messaging.ServiceBus/README.md).  Failure policies can apply delays to further message processing based on delay calculation strategies. 

## Installing Moosesoft.Azure.Messaging.Servicebus

```
dotnet add package Moosesoft.Azure.Messaging.Servicebus
```

## Samples

Sample for [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-service-bus-trigger?tabs=csharp) can be found [here](https://github.com/gtmoose32/moosesoft-azure-messaging-servicebus/tree/master/samples/).

