using MongoDB.Bson;
using UpShop.Api.Utils;
using UpShop.Dominio.Entitys;

namespace UpShop.Api.Models
{
    public class ProductModel : IMapperEntity<Product>
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public void MapperProperties(Product entity)
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Name))
            {
                entity.Name = this.Name;
            }

            if (!string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(Description))
            {
                entity.Description = this.Description;
            }

            if(Quantity != 0)
            {
                entity.Quantity = this.Quantity;
            }

            if(Price != 0)
            {
                entity.Price = this.Price;
            }
        }
    }
}
