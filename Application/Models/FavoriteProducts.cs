using System.ComponentModel.DataAnnotations;

namespace server.Application.Models
{
    public class FavoriteProducts
    {
        [Key]
        public int ID { get; set; }
        public Product? Product { get; set; }
        public Customer? Customer { get; set; }


        public FavoriteProducts() { }
        public FavoriteProducts(Product product, Customer customer)
        {
            Product = product;
            Customer = customer;
        }
    }
}
