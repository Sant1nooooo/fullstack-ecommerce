using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Application.Models;
using server.Infrastructure.Context;

namespace server.Application.Repositories
{
    public class ProductRespository : IProductRepository
    {
        private readonly ECommerceDBContext _context;
        public ProductRespository(ECommerceDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductSubImages>?> GetProductListAsync()
        {
            //IEnumerable<Product>? productList = await _context.Products.ToListAsync();

            IEnumerable<ProductSubImages> products = await _context.ProductSubImages
                .Include(ps => ps.Product)
                .ToListAsync();

            return products;
        }
        public async Task<Product?> GetProductAsync(int productID)
        {
            Product? product = await _context.Products.FindAsync(productID);
            return product;
        }
        public async Task<ProductSubImages?> GetProductWithSubImagesAsync(int productID)
        {
            ProductSubImages? product = await _context.ProductSubImages
                .Where(ps => ps.Product != null && ps.Product.ID == productID)
                .Include(ps => ps.Product)
                .FirstOrDefaultAsync();
            return product;
        }
        public void CreateProductAsync(Product Product)
        {
            _context.Products.Add(Product);
        }
        public void CreateProductSubImagesAsync(ProductSubImages subImages)
        {
            _context.ProductSubImages.Add(subImages);
        }
        public async Task DeleteProductAsync(Product Product)
        {
            IEnumerable<ProductSubImages>? subImages = await _context.ProductSubImages
                .Where(ps => ps.Product != null && ps.Product.ID == Product.ID)
                .ToListAsync();

            _context.ProductSubImages.RemoveRange(subImages); //Removing the sub-images
            _context.Products.Remove(Product); //Removing the specific product to avoid any reference error in the database.
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsProductNameExistingAsync(string ProductName)
        {
            bool IsExisting = await _context.Products.AnyAsync(eachProduct => eachProduct.ProductName == ProductName);
            return IsExisting;
        }
        public async Task UpdateChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
