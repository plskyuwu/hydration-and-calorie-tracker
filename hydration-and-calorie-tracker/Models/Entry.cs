using System;
using LiteDB;

namespace hydration_and_calorie_tracker.Models;

/// <summary>
/// Represents a recorded item entry with a timestamp.
/// </summary>
public record Entry
{
    [BsonId]
    public int Id { get; set; }

    public DateTime Timestamp { get; set; }

    public int ItemId { get; set; }

    public double Amount { get; set; }
}