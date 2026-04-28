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
        var settings = Db.GetCollection<Setting>("settings");
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