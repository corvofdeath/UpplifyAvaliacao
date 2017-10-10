using System;
using System.Collections.Generic;

namespace UpShop.Dominio.Entitys
{
    public class Historic
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
        public DateTime DateTransaction { get; set; }

        // relations
        public IList<ProductDetail> Products { get; set; }

        public Historic()
        {
            DateTransaction = DateTime.Now;
            Products = new List<ProductDetail>();
        }

        public Historic(string description)
        {
            DateTransaction = DateTime.Now;
            Products = new List<ProductDetail>();
            Description = description;
        }

        public void GenerateId()
        {
            Id = Guid.NewGuid();
        }

        public void AddProductsDetail(IEnumerable<ProductDetail> productsDetail)
        {
            Products = new List<ProductDetail>(productsDetail);
        }

        public decimal CalcTotal()
        {
            Total = 0;
            foreach (var product in Products)
            {
                Total += product.CalcTotal();
            }

            return Total;
        }
    }
}
