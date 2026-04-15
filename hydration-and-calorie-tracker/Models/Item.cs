using LiteDB;

namespace hydration_and_calorie_tracker.Models;

/// <summary>
/// Represents a consumable item with caloric and hydration values per unit.
/// </summary>
public record Item
{
    [BsonId]
    public int Id { get; set; }

    public required string Name { get; set; }

    public int CaloriesPerUnit { get; set; }

    public int HydrationPerUnit { get; set; }

    public UnitType UnitType { get; set; }
}