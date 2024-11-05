using server.Application.Models;

namespace server.Application.Interfaces
{
    public interface IFavoriteProductRepository
    {

        Task<FavoriteProducts?> GetSpecificFavoriteProductAsync(int CustomerID, int ProductID);
        Task<IEnumerable<Product?>> GetFavoriteProductAsync(int customerID);
        Task MarkProductAsync(FavoriteProducts favoriteProduct);
        Task DeleteFavoriteProductAsync(FavoriteProducts favoriteProduct);
        Task<bool> IsFavoriteProductExsiting(int ProductID);
    }
}
