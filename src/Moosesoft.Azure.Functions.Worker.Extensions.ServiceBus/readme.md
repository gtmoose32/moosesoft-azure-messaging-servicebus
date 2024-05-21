# Moosesoft.Azure.Functions.Worker.Extensions.ServiceBus

## What is it?

A library for .Net that provides [Azure Functions Isolated Worker Model Service Bus trigger](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-service-bus-trigger?tabs=csharp) support for handling transient exceptions with failure policies that use delay calculator strategies to delay further message processing on retry attempts.