using System.Collections.Generic;

namespace hydration_and_calorie_tracker.Models.Database;

public class TrackingService(ItemRepository items, EntryRepository entries)
{
    private readonly ItemRepository _items = items;

    public void AddEntry(Entry entry) => entries.Add(entry);

    public List<Entry> GetAllEntries() => entries.GetAll();
}