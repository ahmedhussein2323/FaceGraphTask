using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FaceGraphTask.Core.Entities;
using FaceGraphTask.Infrastructure.DbContext;

namespace FaceGraphTask.Infrastructure.Queries
{

    public class Repository<TEntity> where TEntity : BaseEntity
    {
        internal DocumentDbRepository<TEntity> Context;

        public Repository(string collection)
        {
            Context = new DocumentDbRepository<TEntity>(collection);
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            return Context.GetItemsAsync(filter);
            //IQueryable<TEntity> query = DbSet;

            //if (filter != null)
            //{
            //    query = query.Where(filter);
            //}

            //query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            //if (orderBy != null)
            //{
            //    return orderBy(query).ToList();
            //}
            //return query.ToList();
        }

        //public virtual IQueryable<TEntity> AsQueryable()
        //{
        //    return DbSet;
        //}

        //public virtual TEntity GetById(object id)
        //{
        //    return DbSet.Find(id);
        //}

        //public virtual int Count(Expression<Func<TEntity, bool>> filter = null)
        //{
        //    if (filter != null) return DbSet.Count(filter);
        //    return DbSet.Count();
        //}
    }
}
