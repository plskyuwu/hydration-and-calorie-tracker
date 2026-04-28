using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace hydration_and_calorie_tracker.Models.Database;

public class SettingRepository : IRepository<Setting>
{
    private readonly ILiteCollection<Setting> _settings;

    public SettingRepository(DatabaseContext db)
    {
        _settings = db.Db.GetCollection<Setting>();
        _settings.EnsureIndex(s => s.Key);
    }

    public void Add(Setting setting) => _settings.Insert(setting);

    public bool Delete(int id) => _settings.Delete(id);

    public int Count() => _settings.Count();

    public List<Setting> GetAll() => _settings.FindAll().ToList();
}