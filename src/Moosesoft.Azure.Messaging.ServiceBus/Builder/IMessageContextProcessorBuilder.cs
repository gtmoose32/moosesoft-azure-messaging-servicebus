﻿using System;

namespace Moosesoft.Azure.Messaging.ServiceBus.Abstractions.Builder;

/// <summary>
/// Defines a builder for creating instances of <see cref="IMessageContextProcessor"/>.
/// </summary>
public interface IMessageContextProcessorBuilder
{
    /// <summary>
    /// Build a new <see cref="IMessageContextProcessor"/>.
    /// </summary>
    /// <param name="shouldCompleteOn">Optional parameter that allows you to complete certain exceptions without applying failure policies.</param>
    /// <returns><see cref="IMessageContextProcessor"/></returns>
    IMessageContextProcessor Build(Func<Exception, bool> shouldCompleteOn = null);

    IMessageContextProcessor Build(string name, Func<Exception, bool> shouldCompleteOn = null);
}