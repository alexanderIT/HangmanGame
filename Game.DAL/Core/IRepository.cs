using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace Game.DAL.Core
{
    public interface IRepository<T> where T : class, new()
    {
        T Get(Expression<Func<T, bool>> filter);

        IEnumerable<T> GetMany(Expression<Func<T, bool>> filter);

        bool Insert(T item);

        bool Insert(IEnumerable<T> items);

        bool Update(T item, Expression<Func<T, bool>> filter);

        bool Delete(Expression<Func<T, bool>> filter);
    }
}
