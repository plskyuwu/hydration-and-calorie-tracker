using System.Collections.Generic;

namespace hydration_and_calorie_tracker.Models.Database;

/// <summary>
/// A common interface for database repositories.
/// </summary>
/// <typeparam name="T">the type stored inside the repository</typeparam>
/// <typeparam name="TK">the type's key/id type</typeparam>
public interface IRepository<T, in TK>
{
    public void Add(T t);

    public bool Delete(TK tk);

    public int DeleteAll();

    public void Update(T t);

    public int Count();

    public T? GetOne(TK tk);
    
    public List<T> GetAll();
}