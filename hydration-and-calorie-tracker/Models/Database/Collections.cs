namespace hydration_and_calorie_tracker.Models.Database;

/// <summary>
/// Stores the category name strings and other category settings.
/// </summary>
public static class Collections
{
    public static string Items => "items";
    public static string Entries => "entries";
    public static string Settings => "settings";

    public static (int min, int max) DefaultItemIdRange => (1, 16);
}