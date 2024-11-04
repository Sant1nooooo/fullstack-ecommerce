using server.Application.Models;
using server.Application.Models.SeedModel;
using server.Infrastructure.Context;
using System;
using System.Collections.Generic;

namespace server.Infrastructure.Seeder
{
    public class DatabaseSeeder
    {
        private readonly ECommerceDBContext _context;
        private readonly ILogger<DatabaseSeeder> _logger;
        public DatabaseSeeder(ECommerceDBContext context, ILogger<DatabaseSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Seed()
        {
            try
            {
                if (!_context.Products.Any() && !_context.User.Any())
                {
                    //Initial Admin and Customer
                    _context.User.AddRange(
                        new User("jerome","bercero","jerome@gmail.com","123","Admin"),
                        new Customer("santino","esguerra","santino@gmail.com","123")
                    );

                    List<SeederModel> initialProducts = new List<SeederModel>()
                    {
                        new SeederModel()
                        {
                            Product = new Product("Fleece Hoodie","This hoodie exists so you can go big or stay home—and be comfortable either way. It's made from fleece that's smooth on the outside, plush on the inside for that classic cosy feel.","hoodie","https://minimalist-ecommerce-picture.s3.ap-southeast-2.amazonaws.com/picture-1.png",2639),
                            SubImageOne = "https://minimalist-ecommerce-picture.s3.ap-southeast-2.amazonaws.com/picture-1-1.png",
                            SubImageTwo = "https://minimalist-ecommerce-picture.s3.ap-southeast-2.amazonaws.com/picture-1-2.png",
                            SubImageThree = "https://minimalist-ecommerce-picture.s3.ap-southeast-2.amazonaws.com/picture-1-3.png"
                        },

                        new SeederModel()
                        {
                            Product = new Product("Nike Solo Swoosh","A true classic, our Solo Swoosh full-zip hoodie is made from super-soft fleece for smooth, easy comfort. The simplicity of the design lets you pair it up or down for clean, casual daily wear.","hoodie","https://minimalist-ecommerce-picture.s3.ap-southeast-2.amazonaws.com/picture-2.png",4295),
                            SubImageOne = "https://minimalist-ecommerce-picture.s3.ap-southeast-2.amazonaws.com/picture-2-1.png",
                            SubImageTwo = "https://minimalist-ecommerce-picture.s3.ap-southeast-2.amazonaws.com/picture-2-2.png",
                            SubImageThree = "https://minimalist-ecommerce-picture.s3.ap-southeast-2.amazonaws.com/picture-2-3.png"
                        },

                        new SeederModel()
                        {
                            Product = new Product("Festive Fleece Crew","Cosy is the name of the game, and what's cosier than Jordan joggers? Complete with puff-print festive-inspired graphics, this fleece sweatshirt is ready for the season.","hoodie","https://minimalist-ecommerce-picture.s3.ap-southeast-2.amazonaws.com/picture-3.png",1919),
                            SubImageOne = "https://minimalist-ecommerce-picture.s3.ap-southeast-2.amazonaws.com/picture-3-1.png",
                            SubImageTwo = "https://minimalist-ecommerce-picture.s3.ap-southeast-2.amazonaws.com/picture-3-2.png",
                            SubImageThree = "https://minimalist-ecommerce-picture.s3.ap-southeast-2.amazonaws.com/picture-3-3.png"
                        },
                    };
                    //Seeding the initial product and the corresponding subImages to the `Products` and `ProductsImages` sub table.
                    foreach(SeederModel seeder in initialProducts)
                    {
                        _context.Products.Add(seeder.Product!);
                        _context.ProductSubImages.Add(new ProductSubImages(seeder.Product!,seeder.SubImageOne!,seeder.SubImageTwo!,seeder.SubImageThree!));
                    }

                    await _context.SaveChangesAsync();
                }
                Console.WriteLine("MAY LAMAN YUNG DATABASE MO!");
            }
            catch (Exception ex)
            {
                _logger.LogError(message: $"Seeding Error: ({ex.Message})");
            }
        }
    }
}
