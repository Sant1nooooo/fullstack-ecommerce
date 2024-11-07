using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using server.Application.Models;
using System.Data;

namespace server.Infrastructure.Context
{
    public class ECommerceDBContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public ECommerceDBContext(DbContextOptions<ECommerceDBContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<User> User { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSubImages> ProductSubImages { get; set; }
        public DbSet<FavoriteProducts> FavoriteProducts { get; set; }
        public DbSet<CartProducts> CartProducts { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }

        public SqlConnection Connection => new SqlConnection(_configuration["ConnectionStrings:ECommerceConnectionString"]);
    }
}
