using AuthorizationServer.API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AuthorizationServer.API.Repository
{
    public class GenericRepository<T> where T : class
    {
        internal OAuth2DbContext Context;
        internal DbSet<T> DbSet;
        public GenericRepository(OAuth2DbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }
        public virtual IEnumerable<T> Get()
        {
            IQueryable<T> query = DbSet;
            return query.ToList();
        }
        public virtual T GetById(object id)
        {
            return DbSet.Find(id);
        }
        public virtual void Insert(T t)
        {
            DbSet.Add(t);
        }
        public virtual void Delete(object id)
        {
            T t = DbSet.Find(id);
            if(t != null)
            {
                Delete(t);
            }
        }
        public virtual void Delete(T t)
        {
            if(Context.Entry(t).State == EntityState.Detached)
            {
                DbSet.Attach(t);
            }
            DbSet.Remove(t);
        }
        public virtual void Update(T t)
        {
            DbSet.Attach(t);
            Context.Entry(t).State = EntityState.Modified;
        }
        public virtual IEnumerable<T> GetMany(Func<T, Boolean> where)
        {
            return DbSet.Where(where).ToList();
        }
        public virtual IQueryable<T> GetManyQueryable(Func<T, Boolean> where)
        {
            return DbSet.Where(where).AsQueryable();
        }
        public virtual T Get(Func<T, Boolean> where)
        {
            return DbSet.Where(where).FirstOrDefault();
        }
        public virtual void Delete(Func<T, Boolean> where)
        {
            IQueryable<T> listToDelete = DbSet.Where(where).AsQueryable();
            foreach (T item in listToDelete)
            {
                DbSet.Remove(item);
            }
        }
        public virtual IEnumerable<T> GetAll()
        {
            return DbSet.ToList();
        }
        public bool Exists(object id)
        {
            return DbSet.Find(id) != null;
        }
    }
}