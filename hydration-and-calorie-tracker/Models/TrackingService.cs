using System.Collections.Generic;
using hydration_and_calorie_tracker.Models.Repositories;

namespace hydration_and_calorie_tracker.Models;

public class TrackingService(ItemRepository items, EntryRepository entries)
{
    private readonly ItemRepository _items = items;

    public void AddEntry(Entry entry) => entries.Add(entry);

    public List<Entry> GetAllEntries() => entries.GetAll();
}