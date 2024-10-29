using System.ComponentModel.DataAnnotations;

namespace server.Application.Models
{
    public class Voucher
    {
        [Key]
        public int ID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Product? Product { get; set; }
        public Customer? Customer { get; set; }
        public double Discount { get; set; }
        public bool IsUsed { get; set; }


        public Voucher() { }
        public Voucher(string? title, string? description, Product? product, Customer? customer, double discount, bool isUsed)
        {
            Title = title;
            Description = description;
            Product = product;
            Customer = customer;
            Discount = discount;
            IsUsed = isUsed;
        }
    }
}
