using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UpShop.DAL.Interfaces;
using UpShop.Dominio.Interfaces;

namespace UpShop.DAL.Repositories
{
    public class Repository<TContext> : IRepository where TContext : IContext
    {
        protected readonly TContext contexto;

        public Repository(TContext contexto)
        {
            this.contexto = contexto;
        }

        /// <summary>
        /// Mount the query based on the params sent to the methods of query.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderby"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        protected virtual IMongoQueryable<TEntity> GetQueryable<TEntity>(
        Expression<Func<TEntity, bool>> where = null,
        Func<IMongoQueryable<TEntity>, IOrderedMongoQueryable<TEntity>> orderby = null,
        int? skip = null,
        int? take = null)
        where TEntity : class, IEntity, new()
        {
            //Query
            IMongoQueryable<TEntity> query = contexto.Set<TEntity>().AsQueryable();

            if (where != null)
            {
                query = query.Where(where);
            }

            if (orderby != null)
            {
                query = orderby(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        #region CRUD    
        public virtual void Create<TEntity>(TEntity entity, string CreateBy, string Password) where TEntity : class, IEntity, new()
        {
            entity.DateCriation = DateTime.Now;
            entity.CreateBy = CreateBy;
            contexto.Set<TEntity>().InsertOne(entity);
        }

        public virtual Task CreateAsync<TEntity>(TEntity entity, string CreateBy, string Password) where TEntity : class, IEntity, new()
        {
            entity.DateCriation = DateTime.Now;
            entity.CreateBy = CreateBy;
            return contexto.Set<TEntity>().InsertOneAsync(entity);
        }

        public virtual void Update<TEntity>(TEntity entity, string ModifyBy) where TEntity : class, IEntity, new()
        {
            entity.DateModification = DateTime.Now;
            entity.ModifyBy = ModifyBy;
            contexto.Set<TEntity>().ReplaceOne(c => c.Id == entity.Id, entity);
        }

        public virtual Task UpdateAsync<TEntity>(TEntity entity, string ModifyBy) where TEntity : class, IEntity, new()
        {
            entity.DateModification = DateTime.Now;
            entity.ModifyBy = ModifyBy;
            return contexto.Set<TEntity>().ReplaceOneAsync(c => c.Id == entity.Id, entity);
        }

        public virtual void Remove<TEntity>(ObjectId id) where TEntity : class, IEntity, new()
        {
            contexto.Set<TEntity>().DeleteOne(c => c.Id == id);
        }

        public virtual Task RemoveAsync<TEntity>(ObjectId id) where TEntity : class, IEntity, new()
        {
            return contexto.Set<TEntity>().DeleteOneAsync(c => c.Id == id);
        }

        #endregion

        #region Querys
        public virtual IEnumerable<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> where, Func<IMongoQueryable<TEntity>, IOrderedMongoQueryable<TEntity>> orderby, int? skip, int? take) where TEntity : class, IEntity, new()
        {
            return GetQueryable<TEntity>(where, orderby, skip, take).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> where, Func<IMongoQueryable<TEntity>, IOrderedMongoQueryable<TEntity>> orderby, int? skip, int? take) where TEntity : class, IEntity, new()
        {
            return await GetQueryable<TEntity>(where, orderby, skip, take).ToListAsync();
        }

        public virtual bool Exist<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class, IEntity, new()
        {
            return GetQueryable<TEntity>(where).Any();
        }

        public virtual Task<bool> ExistAsync<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class, IEntity, new()
        {
            return GetQueryable<TEntity>(where).AnyAsync();
        }

        public virtual TEntity GetById<TEntity>(ObjectId id) where TEntity : class, IEntity, new()
        {
            return contexto.Set<TEntity>().Find(c => c.Id == id).FirstOrDefault();
        }

        public async virtual Task<TEntity> GetByIdAsync<TEntity>(ObjectId id) where TEntity : class, IEntity, new()
        {
            return await contexto.Set<TEntity>().Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public virtual TEntity GetFirst<TEntity>(Expression<Func<TEntity, bool>> where, Func<IMongoQueryable<TEntity>, IOrderedMongoQueryable<TEntity>> orderby) where TEntity : class, IEntity, new()
        {
            return GetQueryable<TEntity>(where, orderby).FirstOrDefault();
        }

        public virtual async Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> where, Func<IMongoQueryable<TEntity>, IOrderedMongoQueryable<TEntity>> orderby) where TEntity : class, IEntity, new()
        {
            return await GetQueryable<TEntity>(where, orderby).FirstOrDefaultAsync();
        }

        public virtual int Count<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class, IEntity, new()
        {
            return GetQueryable<TEntity>(where).Count();
        }

        public virtual Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class, IEntity, new()
        {
            return GetQueryable<TEntity>(where).CountAsync();
        }


        public virtual TEntity Get<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class, IEntity, new()
        {
            return GetQueryable<TEntity>(where, null).SingleOrDefault();
        }

        public virtual async Task<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class, IEntity, new()
        {
            return await GetQueryable<TEntity>(where, null).SingleOrDefaultAsync();
        }
        #endregion
    }

}
