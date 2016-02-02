using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Scambio.DataAccess.Infrastructure;

namespace Scambio.DataAccess.EntityFramework
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected Repository(ScambioContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<T>();
        }

        protected ScambioContext DbContext { get; }
        protected IDbSet<T> DbSet { get; }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Update(T entity)
        {
            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public void Delete(Expression<Func<T, bool>> conditionExpression)
        {
            var objects = DbSet.Where(conditionExpression).AsEnumerable();
            foreach (var obj in objects)
                DbSet.Remove(obj);
        }

        public T GetById(Guid id)
        {
            return DbSet.Find(id);
        }

        public T Get(Expression<Func<T, bool>> conditionExpression)
        {
            return DbSet.Where(conditionExpression).FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            return DbSet.ToList();
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> conditionExpression)
        {
            return DbSet.Where(conditionExpression).ToList();
        }
    }
}