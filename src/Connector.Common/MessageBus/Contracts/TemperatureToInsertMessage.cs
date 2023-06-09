﻿namespace Connector.Common.MessageBus.Contracts;

/// <summary>
/// Contract of the message sent to the queue to register a temperature reading
/// </summary>
public sealed record TemperatureToInsertMessage
{
    /// <summary>
    /// Unique identifier of the temperature reading
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Identifier of the subject whose the temperature reading belongs to
    /// </summary>
    public Guid PersonId { get; init; }

    /// <summary>
    /// The value of the temperature reading
    /// </summary>
    public decimal Value { get; init; }

    /// <summary>
    /// Timestamp of the temperature reading. Refers to the moment when the reading was taken
    /// </summary>
    public DateTime Timestamp { get; init; }
}