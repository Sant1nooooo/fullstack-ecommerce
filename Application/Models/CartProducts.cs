using System.ComponentModel.DataAnnotations;

namespace server.Application.Models
{
    public class CartProducts
    {
        [Key]
        public int ID { get; set; }
        public Product? Product { get; set; }
        public Customer? Customer { get; set; }
        public bool IsPaid { get; set; }
        public int OriginalPrice { get; set; }
        public int DiscountedPrice { get; set; }
        public int Quantity { get; set; }


        public CartProducts() { }
        public CartProducts(Product Product, Customer Customer, int Price, int Quantity)
        {
            this.Product = Product;
            this.Customer = Customer;
            IsPaid = false;
            this.OriginalPrice = Price;
            this.Quantity = Quantity;
        }
    }
}
