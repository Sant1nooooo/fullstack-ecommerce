using Microsoft.EntityFrameworkCore;
using server.Application.Models;

namespace server.Infrastructure.Context
{
    public class ECommerceDBContext : DbContext
    {
        public ECommerceDBContext(DbContextOptions<ECommerceDBContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSubImages> ProductSubImages { get; set; }
        public DbSet<FavoriteProducts> FavoriteProducts { get; set; }
        public DbSet<CartProducts> CartProducts { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
    }
}
