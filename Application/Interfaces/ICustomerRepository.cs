using server.Application.Models;

namespace server.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task AddProductCartAsync(CartProducts Product);
        Task<CartProducts?> GetCartProductsAsync(int productID, int customerID);
        Task RemoveCartProductAsync(CartProducts CartProduct);
        Task UpdateChangesAsync();
    }
}
