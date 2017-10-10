using MongoDB.Bson;

namespace UpShop.Api.Models
{
    public class ProductDetailModel
    {
        public ObjectId Id { get; set; }
        public int Quantity { get; set; }
    }
}
