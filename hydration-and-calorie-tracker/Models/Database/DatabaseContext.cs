using System;
using System.Globalization;
using LiteDB;

namespace hydration_and_calorie_tracker.Models.Database;

/// <summary>
/// Handles the database connection.
/// </summary>
public class DatabaseContext : IDisposable
{
    public LiteDatabase Db { get; }

    public DatabaseContext(string path)
    {
        Db = new LiteDatabase(path);
        SeedDefaults();
    }

    public void Dispose()
    {
        Db.Dispose();
        GC.SuppressFinalize(this);
    }

    private void SeedDefaults()
    {
        SeedDefaultItems();
        SeedDefaultSettings();
    }

    private void SeedDefaultItems()
    {
        var items = Db.GetCollection<Item>(Collections.Items);
        items.EnsureIndex(i => i.Name);

        var defaultItems = new[]
        {
            new Item
            {
                Id = 1,
                Name = "Water",
                Category = Category.Drink,
                Calories = 0,
                WaterContent = 100,
                Unit = Unit.HundredMilliliters
            },
            new Item
            {
                Id = 2,
                Name = "Rohlík (Czech Bread Roll)",
                Category = Category.Food,
                Calories = 120,
                WaterContent = 0,
                TotalFats = 1.26m,
                SaturatedFats = 0.25m,
                Fiber = 0.84m,
                TotalCarbohydrates = 23.1m,
                Sugar = 0.42m,
                Unit = Unit.Pieces
            }
        };

        foreach (var item in defaultItems)
        {
            if (!items.Exists(i => i.Id == item.Id)) items.Insert(item);
        }

        BumpItemIdSequence(items, Collections.DefaultItemIdRange.max);
    }

    private static void BumpItemIdSequence(ILiteCollection<Item> items,
        int maxReservedId)
    {
        if (items.FindById(maxReservedId) is not null) return;

        var temp = new Item { Id = maxReservedId, Name = "temp" };
        items.Insert(temp);
        items.Delete(maxReservedId);
    }

    private void SeedDefaultSettings()
    {
        var settings = Db.GetCollection<Setting>(Collections.Settings);
        settings.EnsureIndex(s => s.Key);

        var defaultSettings = new[]
        {
            new Setting
            {
                Key = "HydrationGoal",
                Value = ((decimal)3000).ToString(CultureInfo.CurrentCulture)
            },
            new Setting
            {
                Key = "CalorieGoal",
                Value = ((decimal)2000).ToString(CultureInfo.CurrentCulture)
            },
        };

        foreach (var setting in defaultSettings)
        {
            if (!settings.Exists(s => s.Key == setting.Key))
                settings.Insert(setting);
        }
    }
}