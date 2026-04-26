using System.Collections.Generic;

namespace hydration_and_calorie_tracker.Models.Database;

public class TrackingService(
    IRepository<Item> items,
    IRepository<Entry> entries)
{
    private readonly IRepository<Item> _items = items;
    private readonly IRepository<Entry> _entries = entries;

    public void AddEntry(Entry entry) => _entries.Add(entry);

    public List<Entry> GetAllEntries() => _entries.GetAll();
}