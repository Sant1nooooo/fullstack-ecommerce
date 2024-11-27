using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Application.Models;
using server.Infrastructure.Context;
using System.Data;

namespace server.Application.Repositories
{
    public class CustomerRepository: ICustomerRepository
    {
        private readonly ECommerceDBContext _context;
        private readonly SqlConnection connection;
        public CustomerRepository(ECommerceDBContext context)
        {
            _context = context;
            connection = context.Connection;
        }
        public  async Task<IEnumerable<CartProducts>?> GetCartProductListAsync(int customerID)
        {
            IEnumerable<CartProducts>? list = await _context.CartProducts
                .Where(cp => cp.Product != null && cp.Customer!.ID == customerID)
                .Include(cp => cp.Product)
                //.Include(cp => cp.Customer)
                //.GroupBy(cp => cp.Customer)
                //.Select(cp => new {
                //    Customer = cp.Key,
                //    list = cp.Select(cp => cp.Product)
                //})
                .ToListAsync();
            return list;
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
        public async Task<IEnumerable<CartProducts>?> CheckoutCartProductAsync(List<int> ProductIDList, int customerID)
        {
            List<CartProducts> list = new();

            /*SqlParameter customerIDParam = new SqlParameter("@CustomerID", customerID);
            foreach (int eachProductID in ProductIDList)
            {
                CartProducts? currentCartProduct = _context.CartProducts
                    .FromSqlRaw("EXEC SP_CheckoutCartProduct @ProductID, @CustomerID", new SqlParameter("@ProductID", eachProductID), customerIDParam)
                    .AsEnumerable()
                    .FirstOrDefault();
                if (currentCartProduct!.IsPaid)
                {
                    return null;
                }
                currentCartProduct.IsPaid = true;
                list.Add(currentCartProduct);
            }*/
            
            foreach(int eachProductID in ProductIDList)
            {
                CartProducts? currentCartProduct = await GetCartProductsAsync(eachProductID, customerID);
                if (currentCartProduct!.IsPaid) return null;

                currentCartProduct.IsPaid = true;
                list.Add(currentCartProduct);
            }
            await UpdateChangesAsync();
            //Gagawing LINQ para masama yung object ng `Product` and `Customer`.
            return list;
        }
    }
}