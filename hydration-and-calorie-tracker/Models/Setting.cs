namespace hydration_and_calorie_tracker.Models;

/// <summary>
/// Represents a setting's key and value.
/// </summary>
public class Setting
{
    public required string Key { get; set; }
    public required string Value { get; set; }
}