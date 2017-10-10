using System;

namespace UpShop.Dominio.Entitys
{
    public class ProductDetail
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }

        public ProductDetail()
        {
            Total = 0;
            Quantity = 0;
        }

        public ProductDetail(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;

            // We update the product when add a historic. Propagate the change to the real entity is not 
            // responsability of this bussines entity. For optimization purposes.
            UpdateQuantity();
        }

        public decimal CalcTotal()
        {
            Total = Quantity * Product.Price;
            return Total;
        }

        private void UpdateQuantity()
        {
            var total = Product.Quantity - Quantity;
            Product.Quantity = Math.Max(0, total);
        }
    }
}
