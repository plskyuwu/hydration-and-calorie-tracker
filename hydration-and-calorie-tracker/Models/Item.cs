using LiteDB;

namespace hydration_and_calorie_tracker.Models;

/// <summary>
/// Represents a consumable item with caloric, hydration and other nutritional values per unit.
/// </summary>
public record Item
{
    [BsonId] public int Id { get; set; }

    public required string Name { get; set; }

    public decimal Calories { get; set; }

    public decimal WaterContent { get; set; }

    public decimal TotalFats { get; set; }

    public decimal SaturatedFats { get; set; }

    public decimal TotalCarbohydrates { get; set; }

    public decimal Sugar { get; set; }

    public decimal Protein { get; set; }

    public decimal Salt { get; set; }

    public Category Category { get; set; }

    public Unit Unit { get; set; }
}