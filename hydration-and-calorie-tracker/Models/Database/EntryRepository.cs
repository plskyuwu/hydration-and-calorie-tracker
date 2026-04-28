using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace hydration_and_calorie_tracker.Models.Database;

public class EntryRepository : IRepository<Entry, int>
{
    private readonly ILiteCollection<Entry> _entries;

    public EntryRepository(DatabaseContext db)
    {
        _entries = db.Db.GetCollection<Entry>("entries");
        _entries.EnsureIndex(e => e.Timestamp);
    }

    public void Add(Entry entry) => _entries.Insert(entry);

    public bool Delete(int id) => _entries.Delete(id);

    public void Update(Entry entry) => _entries.Upsert(entry);

    public int Count() => _entries.Count();

    public Entry? GetOne(int id) => _entries.FindById(id);

    public List<Entry> GetAll() => _entries.FindAll().ToList();
}