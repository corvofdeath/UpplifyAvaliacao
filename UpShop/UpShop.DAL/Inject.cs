using UpShop.DAL.Context;
using UpShop.DAL.Repositories;
using UpShop.Dominio.Interfaces;

namespace UpShop.DAL
{
    public static class Inject
    {
        public static IRepository GetRepositoryImplemantation(string connectionString)
        {
            return new Repository<UpShopContext>(new UpShopContext(connectionString));
        }
    }

}
