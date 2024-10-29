using System.ComponentModel.DataAnnotations;

namespace server.Application.Models
{
    public class ProductSubImages
    {
        [Key]
        public int ID { get; set; }
        public Product? Product { get; set; }
        public string? subImageOne { get; set; }
        public string? subImageTwo { get; set; }
        public string? subImageThree { get; set; }


        public ProductSubImages() { }
        public ProductSubImages(Product Product, string subImageOne, string subImageTwo, string subImageThree)
        {
            this.Product = Product;
            this.subImageOne = subImageOne;
            this.subImageTwo = subImageTwo;
            this.subImageThree = subImageThree;
        }
    }
}
