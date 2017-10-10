using MongoDB.Bson;
using System.Collections.Generic;

namespace UpShop.Api.Models
{
    public class ShoppingCartModel
    {
        public ObjectId UserId { get; set; }
        public IList<ProductDetailModel> Products { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
    }
}
