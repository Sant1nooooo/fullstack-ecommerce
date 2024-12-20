﻿using System.ComponentModel.DataAnnotations;

namespace server.Application.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDetails { get; set; }
        public string? ImageURL { get; set; }
        public string? Type { get; set; }
        public int AvailableQuantity{ get; set; }
        public int OriginalPrice { get; set; }
        public int DiscountedPrice { get; set; }
        public int UnitSold { get; set; }
        public int Discount { get; set; }
        public bool IsAvailable { get; set; }

        public Product(string ProductName, string ProductDetails, string Type, string ImageURL, int OriginalPrice)
        {
            this.ProductName = ProductName;
            this.ProductDetails = ProductDetails;
            this.Type = Type;
            this.ImageURL = ImageURL;
            AvailableQuantity = 10;
            this.OriginalPrice = OriginalPrice;
            DiscountedPrice = 0;
            UnitSold = 0;
            IsAvailable = true;
            Discount = 0;
        }
    }
}
