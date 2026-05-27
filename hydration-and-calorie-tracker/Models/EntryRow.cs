namespace hydration_and_calorie_tracker.Models;

/// <summary>
/// A simple wrapper record that couples entry and the entry's item name into one object
/// </summary>
/// <param name="Entry">entry</param>
/// <param name="ItemName">entry's item name</param>
public record EntryRow(Entry Entry, string ItemName);