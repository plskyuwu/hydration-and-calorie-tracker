using System.Collections.Generic;

namespace hydration_and_calorie_tracker.Models.Database;

public class TrackingService(
    IRepository<Item, int> items,
    IRepository<Entry, int> entries)
{
    private readonly IRepository<Item, int> _items = items;
    private readonly IRepository<Entry, int> _entries = entries;

    public void AddEntry(Entry entry) => _entries.Add(entry);

    public List<Entry> GetAllEntries() => _entries.GetAll();
}