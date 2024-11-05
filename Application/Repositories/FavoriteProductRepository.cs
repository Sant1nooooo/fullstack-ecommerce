using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Application.Models;
using server.Infrastructure.Context;

namespace server.Application.Repositories
{
    public class FavoriteProductRepository : IFavoriteProductRepository
    {
        private readonly ECommerceDBContext _context;
        public FavoriteProductRepository(ECommerceDBContext context)
        {
            _context = context;
        }

        public async Task<FavoriteProducts?> GetSpecificFavoriteProductAsync(int CustomerID, int ProductID)
        {
            FavoriteProducts? favoriteProduct = await _context.FavoriteProducts
                .Where(fp => fp.Product!.ID == ProductID && fp.Customer!.ID == CustomerID)
                .FirstOrDefaultAsync();
            return favoriteProduct;
        }
        public async Task<IEnumerable<Product?>> GetFavoriteProductAsync(int customerID)
        {
            IEnumerable<Product?> favoriteProductList = await _context.FavoriteProducts
                .Where(fp => fp.Customer != null && fp.Customer.ID == customerID)
                .Include(fp => fp.Product)
                .Select(fp => fp.Product)
                .ToListAsync();

            return favoriteProductList;
        }
        public async Task MarkProductAsync(FavoriteProducts favoriteProduct)
        {
            _context.FavoriteProducts.Add(favoriteProduct);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteFavoriteProductAsync(FavoriteProducts favoriteProduct)
        {
            _context.FavoriteProducts.Remove(favoriteProduct);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsFavoriteProductExsiting(int ProductID)
        {
            bool isExisting = await _context.FavoriteProducts.AnyAsync(product => product.Product!.ID == ProductID);
            Console.WriteLine(isExisting);
            return isExisting;
        }
    }
}
