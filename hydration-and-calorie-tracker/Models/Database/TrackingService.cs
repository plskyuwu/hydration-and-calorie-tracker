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
    public decimal HydrationGoal
    {
        get => decimal.Parse(settings.GetOne("HydrationGoal")!.Value);
        set => settings.Update(new Setting
        {
            Key = "HydrationGoal",
            Value = value.ToString(CultureInfo.CurrentCulture)
        });
    }

    public decimal CalorieGoal
    {
        get => decimal.Parse(settings.GetOne("CalorieGoal")!.Value);
        set => settings.Update(new Setting
        {
            Key = "CalorieGoal",
            Value = value.ToString(CultureInfo.CurrentCulture)
        });
    }

    public decimal TotalHydrationToday => GetTodayEntries()
        .Select(e => new { Entry = e, Item = items.GetOne(e.ItemId) })
        .Where(p => p.Item is not null)
        .Sum(p => p.Item!.WaterContent * p.Entry.Amount /
                  (p.Item!.Unit == Unit.HundredMilliliters ||
                   p.Item!.Unit == Unit.HundredMilliliters
                      ? 100
                      : 1));

    public decimal TotalCaloriesToday => GetTodayEntries()
        .Select(e => new { Entry = e, Item = items.GetOne(e.ItemId) })
        .Where(p => p.Item is not null)
        .Sum(p => p.Item!.Calories * p.Entry.Amount /
                  (p.Item!.Unit == Unit.HundredMilliliters ||
                   p.Item!.Unit == Unit.HundredMilliliters
                      ? 100
                      : 1));

    public void AddEntry(Entry entry) => entries.Add(entry);

    public void AddItem(Item item) => items.Add(item);

    public bool DeleteEntry(int id) => entries.Delete(id);

    public bool DeleteItem(int id) => items.Delete(id);

    public int DeleteAllEntries() => entries.DeleteAll();

    public int DeleteAllItems() => items.DeleteAll();

    public List<Entry> GetTodayEntries()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        return GetEntriesByDateRange(today, tomorrow);
    }

    public List<Entry> GetEntriesByDateRange(DateTime from, DateTime to)
    {
        if (entries is EntryRepository entryRepository)
        {
            return entryRepository.GetByDateRange(from, to);
        }

        return GetAllEntries()
            .Where(e => e.Timestamp >= from && e.Timestamp < to).ToList();
    }

    public List<Entry> GetAllDrinkEntries()
    {
        var drinkIds = GetAllDrinks().Select(i => i.Id).ToHashSet();
        return GetAllEntries().Where(e => drinkIds.Contains(e.ItemId)).ToList();
    }

    public List<Entry> GetAllFoodEntries()
    {
        var foodIds = GetAllItems().Select(i => i.Id).ToHashSet();
        return GetAllEntries().Where(e => foodIds.Contains(e.ItemId)).ToList();
    }

    public List<Entry> GetAllEntries() => entries.GetAll();

    public Item? GetItem(int id) => items.GetOne(id);
    
    public List<Item> GetAllDrinks()
    {
        if (items is ItemRepository itemRepository)
        {
            return itemRepository.GetAllDrinks();
        }

        return items.GetAll().Where(i => i.Category == Category.Drink)
            .ToList();
    }

    public List<Item> GetAllFoods()
    {
        if (items is ItemRepository itemRepository)
        {
            return itemRepository.GetAllFoods();
        }

        return items.GetAll().Where(i => i.Category == Category.Food)
            .ToList();
    }

    public List<Item> GetAllItems() => items.GetAll();
}