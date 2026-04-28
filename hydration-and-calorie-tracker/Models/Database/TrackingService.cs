using System.Collections.Generic;
using System.Globalization;

namespace hydration_and_calorie_tracker.Models.Database;

public class TrackingService(
    IRepository<Item, int> items,
    IRepository<Entry, int> entries,
    IRepository<Setting, string> settings)
{
    private readonly IRepository<Item, int> _items = items;
    private readonly IRepository<Entry, int> _entries = entries;
    private readonly IRepository<Setting, string> _settings = settings;

    public decimal HydrationGoal
    {
        get => decimal.Parse(_settings.GetOne("HydrationGoal")!.Value);
        set => _settings.Update(new Setting
        {
            Key = "HydrationGoal",
            Value = value.ToString(CultureInfo.CurrentCulture)
        });
    }

    public decimal CalorieGoal
    {
        get => decimal.Parse(_settings.GetOne("CalorieGoal")!.Value);
        set => _settings.Update(new Setting
        {
            Key = "CalorieGoal",
            Value = value.ToString(CultureInfo.CurrentCulture)
        });
    }

    public void AddEntry(Entry entry) => _entries.Add(entry);

    public List<Entry> GetAllEntries() => _entries.GetAll();
}