using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace hydration_and_calorie_tracker.Models.Database;

public class SettingRepository : IRepository<Setting, string>
{
    private readonly ILiteCollection<Setting> _settings;

    public SettingRepository(DatabaseContext db)
    {
        _settings = db.Db.GetCollection<Setting>("settings");
        _settings.EnsureIndex(s => s.Key);
    }

    public void Add(Setting setting) => _settings.Insert(setting);

    public bool Delete(string key) => _settings.DeleteMany(s => s.Key == key) > 0;

    public void Update(Setting setting) => _settings.Upsert(setting);

    public int Count() => _settings.Count();

    public Setting? GetOne(string key) => _settings.FindOne(s => s.Key == key);

    public List<Setting> GetAll() => _settings.FindAll().ToList();
}