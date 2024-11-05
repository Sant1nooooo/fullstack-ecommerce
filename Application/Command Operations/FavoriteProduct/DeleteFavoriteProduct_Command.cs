using MediatR;
using server.Application.Interfaces;
using server.Application.Models;
using System.Text.Json.Serialization.Metadata;
using static server.Core.ResponseModels;

namespace server.Application.Command_Operations.FavoriteProduct
{
    public class DeleteFavoriteProduct_Command : IRequest<DeleteFavoriteProduct_Result>
    {
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
    }
    public class DeleteFavoriteProduct_CommandHandler : IRequestHandler<DeleteFavoriteProduct_Command, DeleteFavoriteProduct_Result>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IProductRepository _productRepository;
        private readonly IFavoriteProductRepository _favoriteRepository;
        public DeleteFavoriteProduct_CommandHandler(IUsersRepository usersRepository, IProductRepository productRepository, IFavoriteProductRepository favoriteRepository)
        {
            _usersRepository = usersRepository;
            _productRepository = productRepository;
            _favoriteRepository = favoriteRepository;
        }

        public async Task<DeleteFavoriteProduct_Result> Handle(DeleteFavoriteProduct_Command request, CancellationToken ct)
        {
            Customer? selectedCustomer = await _usersRepository.GetCustomerAsync(request.CustomerID);
            Product? selectedProduct = await _productRepository.GetProductAsync(request.ProductID);

            if (selectedCustomer is null) return new DeleteFavoriteProduct_Result() { IsDeleted = false, Message = "WARNING: Invalid customerID!" };
            if (selectedProduct is null) return new DeleteFavoriteProduct_Result() { IsDeleted = false, Message = "WARNING: Invalid productID!" };

            FavoriteProducts? favoriteProduct = await _favoriteRepository.GetSpecificFavoriteProductAsync(request.CustomerID, request.ProductID);
            if(favoriteProduct is null) return new DeleteFavoriteProduct_Result() { IsDeleted = false, Message = "WARNING: This product is not in your favorites list!" };

            await _favoriteRepository.DeleteFavoriteProductAsync(favoriteProduct);

            return new DeleteFavoriteProduct_Result() { IsDeleted = false, Message = $"{selectedProduct.ProductName}({selectedProduct.Type}) is successfully deleted in your favorites list!" };
        }
    }
}
