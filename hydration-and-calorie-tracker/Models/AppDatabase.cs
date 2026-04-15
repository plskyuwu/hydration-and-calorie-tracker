using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace hydration_and_calorie_tracker.Models;

public class AppDatabase : IDisposable
{
    private readonly LiteDatabase _db;

    private ILiteCollection<Item> Items => _db.GetCollection<Item>("items");

    private ILiteCollection<Entry> Entries =>
        _db.GetCollection<Entry>("entries");

    public AppDatabase(string path)
    {
        _db = new LiteDatabase(path);

        Items.EnsureIndex(i => i.Name);
        Entries.EnsureIndex(e => e.Timestamp);
    }

    public void AddItem(Item item)
    {
        Items.Insert(item);
    }

    public void AddEntry(Entry entry)
    {
        Entries.Insert(entry);
    }

    public List<Item> GetAllItems()
    {
        return Items.FindAll().ToList();
    }

    public List<Entry> GetAllEntries()
    {
        return Entries.FindAll().ToList();
    }

    public void Dispose()
    {
        _db.Dispose();
        GC.SuppressFinalize(this);
    }
}