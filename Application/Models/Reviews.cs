using System.ComponentModel.DataAnnotations;

namespace server.Application.Models
{
    public class Reviews
    {
        [Key]
        public int ID { get; set; }
        public Product? Product { get; set; }
        public Customer? Customer { get; set; }
        public int Rate { get; set; }
        public string? Comment { get; set; }
        public Reviews() { }
        public Reviews(Product Product, Customer Customer, int Rate, string Comment)
        {
            this.Product = Product;
            this.Customer = Customer;
            this.Rate = Rate;
            this.Comment = Comment;
        }
    }
}
