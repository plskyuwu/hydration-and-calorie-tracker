using System.Collections.Generic;

namespace hydration_and_calorie_tracker.Models.Database;

public interface IRepository<T>
{
    public void Add(T t);

    public bool Delete(int id);

    public int Count();

    public List<T> GetAll();
}