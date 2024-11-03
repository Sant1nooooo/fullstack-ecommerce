using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Application.Models;
using server.Infrastructure.Context;

namespace server.Application.Repositories
{
    public class CustomerRepository: ICustomerRepository
    {
        private readonly ECommerceDBContext _context;
        public CustomerRepository(ECommerceDBContext context)
        {
            _context = context;
        }
        public async Task AddProductCartAsync(CartProducts Product)
        {
            _context.CartProducts.Add(Product);
            //await _context.SaveChangesAsync();
            await UpdateChangesAsync();
        }
        public async Task<CartProducts?> GetCartProductsAsync(int cartProductUD, int customerID)
        {
            CartProducts? selectedCartProduct = await _context.CartProducts
                .Where(cp => (cp.Product != null && cp.Product.ID == cartProductUD) && (cp.Customer != null && cp.Customer.ID == customerID))
                .Include(cp => cp.Product)
                .Include(cp => cp.Customer)
                .FirstOrDefaultAsync();

            return selectedCartProduct;
        }
        public async Task RemoveCartProductAsync(CartProducts CartProduct)
        {
            _context.CartProducts.Remove(CartProduct);
            await UpdateChangesAsync();
        }
        public async Task UpdateChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
