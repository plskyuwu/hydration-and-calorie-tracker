using System.Collections.Generic;

namespace hydration_and_calorie_tracker.Models.Database;

public interface IRepository<T, in TK>
{
    public void Add(T t);

    public bool Delete(TK tk);

    public void Update(T t);

    public int Count();

    public T? GetOne(TK tk);
    
    public List<T> GetAll();
}