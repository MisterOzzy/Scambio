using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Scambio.DataAccess.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> conditionExpression);
        T GetById(Guid id);
        T Get(Expression<Func<T, bool>> conditionExpression);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> conditionExpression);
    }
}