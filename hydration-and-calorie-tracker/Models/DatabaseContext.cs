using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace hydration_and_calorie_tracker.Models;

public class DatabaseContext : IDisposable
{
    public LiteDatabase Db { get; }

    public DatabaseContext(string path)
    {
        Db = new LiteDatabase(path);
    }

    public void Dispose()
    {
        Db.Dispose();
        GC.SuppressFinalize(this);
    }
}