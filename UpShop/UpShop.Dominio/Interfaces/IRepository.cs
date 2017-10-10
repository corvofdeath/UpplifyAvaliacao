using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UpShop.Dominio.Interfaces
{
    /// <summary>
    /// Genery Repository to all entitys in the Domain Layer.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Return a list of entitys based in query params.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where">Filter param of the query.</param>
        /// <param name="orderby">Oder param of the query.</param>
        /// <param name="skip">Skip elements.</param>
        /// <param name="take">Take elements.</param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll<TEntity>(
            Expression<Func<TEntity, bool>> where = null,
            Func<IMongoQueryable<TEntity>, IOrderedMongoQueryable<TEntity>> orderby = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Return a list of entitys based in query params. Run async.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where">Filter param of the query.</param>
        /// <param name="orderby">Oder param of the query.</param>
        /// <param name="skip">Skip elements.</param>
        /// <param name="take">Take elements.</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            Expression<Func<TEntity, bool>> where = null,
            Func<IMongoQueryable<TEntity>, IOrderedMongoQueryable<TEntity>> orderby = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Return a entity based in query params.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where">Filter of the query.</param>
        /// <returns></returns>
        TEntity Get<TEntity>(
            Expression<Func<TEntity, bool>> where = null)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Return a entity based in query params.Run async.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where">Filter of the query.</param>
        /// <returns></returns>
        Task<TEntity> GetAsync<TEntity>(
            Expression<Func<TEntity, bool>> where = null)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Return the first entity found in database, based in query params. 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where">Filter param of the query.</param>
        /// <param name="orderby">Oder param of the query.</param>
        /// <returns></returns>
        TEntity GetFirst<TEntity>(
            Expression<Func<TEntity, bool>> where = null,
            Func<IMongoQueryable<TEntity>, IOrderedMongoQueryable<TEntity>> orderby = null)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Return the first entity found in database, based in query params. Run async.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where">Filter param of the query.</param>
        /// <param name="orderby">Oder param of the query.</param>
        /// <returns></returns>
        Task<TEntity> GetFirstAsync<TEntity>(
            Expression<Func<TEntity, bool>> where = null,
            Func<IMongoQueryable<TEntity>, IOrderedMongoQueryable<TEntity>> orderby = null)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Return a entity based on Id.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id">Id of the entity.</param>
        /// <returns></returns>
        TEntity GetById<TEntity>(ObjectId id)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Return a entity based on Id. Run async.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id">Id of the entity.</param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync<TEntity>(ObjectId id)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Returns the count of entitys in a collection.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where">Filter of the count.</param>
        /// <returns></returns>
        int Count<TEntity>(Expression<Func<TEntity, bool>> where = null)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Returns the count of entitys in a collection. Run async.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where">Filter of the count.</param>
        /// <returns></returns>
        Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> where = null)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Verify if the entity exist in database. Returns true or false.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where">Filter of the query.</param>
        /// <returns></returns>
        bool Exist<TEntity>(Expression<Func<TEntity, bool>> where = null)
            where TEntity : class, IEntity, new();


        /// <summary>
        /// Verify if the entity exist in database. Returns true or false. Run async.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where">Filter of the query.</param>
        /// <returns></returns>
        /// <returns></returns>
        Task<bool> ExistAsync<TEntity>(Expression<Func<TEntity, bool>> where = null)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Create a register in database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">Entity.</param>
        /// <param name="createBy">Creator.</param>
        void Create<TEntity>(TEntity entity, string createBy = null, string Password = null)
        where TEntity : class, IEntity, new();

        /// <summary>
        /// Create a register in database. Run async.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">Entity.</param>
        /// <param name="createBy">Creator.</param>
        Task CreateAsync<TEntity>(TEntity entity, string createBy = null, string Password = null)
        where TEntity : class, IEntity, new();

        /// <summary>
        /// Update a register in database. 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">Entity.</param>
        /// <param name="createBy">Modifier.</param>
        void Update<TEntity>(TEntity entity, string modifyBy = null)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Update a register in database. Run async.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">Entity.</param>
        /// <param name="createBy">Modifier.</param>
        Task UpdateAsync<TEntity>(TEntity entity, string modifyBy = null)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Remove a register of the database throught your id.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id">Id of the register</param>
        void Remove<TEntity>(ObjectId id)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Remove a register of the database throught your id. Run async.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id">Id of the register</param>
        Task RemoveAsync<TEntity>(ObjectId id)
            where TEntity : class, IEntity, new();
    }

}
