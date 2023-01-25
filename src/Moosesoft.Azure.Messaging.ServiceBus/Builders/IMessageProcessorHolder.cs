namespace Moosesoft.Azure.Messaging.ServiceBus.Builders;

/// <summary>
/// Defines a set of methods for holding different implementations of <see cref="IMessageProcessor"/>.
/// </summary>
public interface IMessageProcessorHolder
{
    /// <summary>
    /// Set an instance of <see cref="IMessageProcessor"/> to be used for configuration.
    /// </summary>
    /// <param name="messageProcessor"><see cref="IMessageProcessor"/> instance to be used.</param>
    /// <typeparam name="TProcessor">Type of <see cref="IMessageProcessor"/></typeparam>
    /// <returns><see cref="IFailurePolicyHolder"/></returns>
    IFailurePolicyHolder WithMessageProcessor<TProcessor>(TProcessor messageProcessor)
        where TProcessor : IMessageProcessor;

    /// <summary>
    /// Set an instance of <see cref="IMessageProcessor"/> to be used for configuration.
    /// </summary>
    /// <typeparam name="TProcessor">Type of <see cref="IMessageProcessor"/></typeparam>
    /// <returns><see cref="IFailurePolicyHolder"/></returns>
    IFailurePolicyHolder WithMessageProcessor<TProcessor>()
        where TProcessor : IMessageProcessor, new();

    /// <summary>
    /// Set a function pointer for message processing to be used for configuration instead of an instance of <see cref="IMessageProcessor"/>.
    /// </summary>
    /// <returns><see cref="IFailurePolicyHolder"/></returns>
    IFailurePolicyHolder WithMessageProcessor(Func<ServiceBusReceivedMessage, CancellationToken, Task> processMessage);
}