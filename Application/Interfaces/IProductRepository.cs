using server.Application.Models;

namespace server.Application.Interfaces
{
    public interface IProductRepository
    {

        Task<IEnumerable<ProductSubImages>?> GetProductListAsync();
        Task<Product?> GetProductAsync(int productID);
        Task<ProductSubImages?> GetProductWithSubImagesAsync(int productID);
        void CreateProductAsync(Product Product);
        void CreateProductSubImagesAsync(ProductSubImages SubImages);
        Task DeleteProductAsync(Product Product);
        Task UpdateChanges();
        Task<bool> IsProductNameExistingAsync(string ProductName);
    }
}
