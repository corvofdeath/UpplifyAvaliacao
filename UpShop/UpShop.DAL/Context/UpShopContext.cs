using MongoDB.Driver;
using System.Reflection;
using UpShop.DAL.Interfaces;
using UpShop.Dominio.Helpers;
using UpShop.Dominio.Interfaces;

namespace UpShop.DAL.Context
{
    public class UpShopContext : IContext
    {
        private readonly IMongoDatabase database;

        public IMongoDatabase DataBase { get { return database; } }

        public UpShopContext(string connectionString)
        {
            var cliente = new MongoClient(connectionString);

            // split string to get the database name on last position
            var splitString = connectionString.Split('/');
            var databaseName = splitString[splitString.Length - 1];

            database = cliente.GetDatabase(databaseName);
        }

        public IMongoCollection<TEntity> Set<TEntity>() where TEntity : class, IEntity, new()
        {
            var collection = typeof(TEntity).GetTypeInfo().GetCustomAttribute<CollectionAttribute>();

            return database.GetCollection<TEntity>(collection.Name);
        }
    }
}
