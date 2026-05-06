using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LiteDB;

namespace hydration_and_calorie_tracker.Models.Database;

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

        var defaultDrinks = new[]
        {
            new Item
            {
                Name = "Water",
                Calories = 0,
                WaterContent = 100,
                Unit = Unit.HundredMilliliters
            }
        };

        var defaultFoods = new[]
        {
            new Item
            {
                Name = "Rohlík (Czech Bread Roll)",
                Calories = 120,
                WaterContent = 0,
                TotalFats = 1.26m,
                SaturatedFats = 0.25m,
                Fibers = 0.84m,
                TotalCarbohydrates = 23.1m,
                Sugar = 0.42m,
                Unit = Unit.Pieces
            }
        };

        foreach (var drink in defaultDrinks)
        {
            if (items.Exists(i => i.Name == drink.Name)) break;

            drink.Category = Category.Drink;

            items.Insert(drink);
        }

        foreach (var food in defaultFoods)
        {
            if (items.Exists(i => i.Name == food.Name)) break;

            food.Category = Category.Food;

            items.Insert(food);
        }
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