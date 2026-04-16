using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace hydration_and_calorie_tracker.Models.Repositories;

public class EntryRepository
{
    private readonly ILiteCollection<Entry> _entries;

    public EntryRepository(DatabaseContext db)
    {
        _entries = db.Db.GetCollection<Entry>("entries");
        _entries.EnsureIndex(e => e.Timestamp);
    }

    public void Add(Entry entry) => _entries.Insert(entry);

    public List<Entry> GetAll() => _entries.FindAll().ToList();
}