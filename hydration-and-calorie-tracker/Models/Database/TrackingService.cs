using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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

    public decimal TotalHydrationToday => GetTodayEntries()
        .Select(e => new { Entry = e, Item = _items.GetOne(e.ItemId) })
        .Where(p => p.Item is not null)
        .Sum(p => p.Item!.WaterContent * p.Entry.Amount);

    public decimal TotalCaloriesToday => GetTodayEntries()
        .Select(e => new { Entry = e, Item = _items.GetOne(e.ItemId) })
        .Where(p => p.Item is not null)
        .Sum(p => p.Item!.Calories * p.Entry.Amount);

    public void AddEntry(Entry entry) => _entries.Add(entry);

    public List<Entry> GetTodayEntries()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        return GetEntriesByDateRange(today, tomorrow);
    }

    public List<Entry> GetEntriesByDateRange(DateTime from, DateTime to)
    {
        if (_entries is EntryRepository entryRepository)
        {
            return entryRepository.GetByDateRange(from, to);
        }

        return GetAllEntries()
            .Where(e => e.Timestamp >= from && e.Timestamp < to).ToList();
    }

    public List<Entry> GetAllEntries() => _entries.GetAll();
}