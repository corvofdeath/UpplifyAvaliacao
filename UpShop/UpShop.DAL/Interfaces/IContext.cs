using MongoDB.Driver;
using UpShop.Dominio.Interfaces;

namespace UpShop.DAL.Interfaces
{
    /// <summary>
    /// Mid-Layer between the repository and the Mongo Driver.
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// Mongo-Driver database object.
        /// </summary>
        IMongoDatabase DataBase { get; }

        /// <summary>
        /// Get dynamic collection on database object based on a type of entity.
        /// </summary>
        /// <typeparam name="TEntidade">A Entity type who provide a CollectionAttribute meta-data</typeparam>
        /// <returns></returns>
        IMongoCollection<TEntity> Set<TEntity>() where TEntity : class, IEntity, new();
    }

}
