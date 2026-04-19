using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace hydration_and_calorie_tracker.Models.Database;

public class ItemRepository
{
    private readonly ILiteCollection<Item> _items;

    public ItemRepository(DatabaseContext db)
    {
        _items = db.Db.GetCollection<Item>("items");
        _items.EnsureIndex(i => i.Name);
    }

    public void Add(Item item) => _items.Insert(item);

    public List<Item> GetAll() => _items.FindAll().ToList();
}