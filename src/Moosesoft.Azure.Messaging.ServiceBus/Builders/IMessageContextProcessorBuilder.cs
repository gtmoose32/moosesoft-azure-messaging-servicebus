﻿namespace Moosesoft.Azure.Messaging.ServiceBus.Builders;

/// <summary>
/// Defines a builder for creating instances of <see cref="IMessageContextProcessor"/>.
/// </summary>
public interface IMessageContextProcessorBuilder
{
    /// <summary>
    /// Build a new <see cref="IMessageContextProcessor"/>.
    /// </summary>
    /// <param name="shouldCompleteOn">Optional parameter that allows you to complete certain exceptions without applying failure policies.</param>
    /// <param name="name">Name of the <see cref="IMessageContextProcessor"/> to build.</param>
    /// <returns><see cref="IMessageContextProcessor"/></returns>
    IMessageContextProcessor Build(Func<Exception, bool> shouldCompleteOn = null, string name = "default");
}