using System.ComponentModel.DataAnnotations;

namespace server.Application.Models
{
    public class Voucher
    {
        [Key]
        public int ID { get; set; } //EF Core
        public string? Title { get; set; } //Provided
        public string? Description { get; set; } //Provided
        public Product? Product { get; set; } //Provided
        public Customer? Customer { get; set; } // Calculated
        public double Discount { get; set; } //Provided
        public bool IsUsed { get; set; } //Instiantiated
        public DateTime? CreatedAt { get; set; } //SP


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
