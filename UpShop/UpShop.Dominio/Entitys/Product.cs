using UpShop.Dominio.Helpers;

namespace UpShop.Dominio.Entitys
{
    [Collection("Products")]
    public class Product : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
