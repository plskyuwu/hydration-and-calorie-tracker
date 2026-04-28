using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace hydration_and_calorie_tracker.Models.Database;

public class ItemRepository : IRepository<Item, int>
{
    private readonly ILiteCollection<Item> _items;

    public ItemRepository(DatabaseContext db)
    {
        _items = db.Db.GetCollection<Item>("items");
        _items.EnsureIndex(i => i.Name);
    }

    public void Add(Item item) => _items.Insert(item);

    public bool Delete(int id) => _items.Delete(id);

    public void Update(Item item) => _items.Upsert(item);

    public int Count() => _items.Count();

    public Item? GetOne(int id) => _items.FindById(id);

    public List<Item> GetAll() => _items.FindAll().ToList();
}