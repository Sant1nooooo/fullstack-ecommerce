using MediatR;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using server.Application.Interfaces;
using server.Application.Models;
using static server.Core.ResponseModels;

namespace server.Application.Command_Operations.FavoriteProduct
{
    public class MarkFavoriteProduct_Command : IRequest<FavoriteProduct_Result>
    {
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
    }
    public class MarkFavoriteProduct_CommandHandler : IRequestHandler<MarkFavoriteProduct_Command, FavoriteProduct_Result>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IProductRepository _productRepository;
        private readonly IFavoriteProductRepository _favoriteRepository;
        public MarkFavoriteProduct_CommandHandler(IUsersRepository usersRepository, IProductRepository productRepository, IFavoriteProductRepository favoriteRepository)
        {
            _usersRepository = usersRepository;
            _productRepository = productRepository;
            _favoriteRepository = favoriteRepository;
        }

        public async Task<FavoriteProduct_Result> Handle(MarkFavoriteProduct_Command request, CancellationToken ct)
        {
            Customer? selectedCustomer = await _usersRepository.GetCustomerAsync(request.CustomerID);
            Product? selectedProduct = await _productRepository.GetProductAsync(request.ProductID);

            if(selectedCustomer is null) return new FavoriteProduct_Result() { IsMarked = false , Message = "WARNING: Invalid customerID!"};
            if (selectedProduct is null) return new FavoriteProduct_Result() { IsMarked = false, Message = "WARNING: Invalid productID!" };

            if(await _favoriteRepository.IsFavoriteProductExsiting(request.ProductID))
            {
                return new FavoriteProduct_Result() { IsMarked = false, Message = $"WARNING: {selectedProduct.ProductName} is already added to your favorites list!" };
            }

            await _favoriteRepository.MarkProductAsync(new FavoriteProducts(selectedProduct, selectedCustomer));

            return new FavoriteProduct_Result() { IsMarked = true, Message = $"You've successfully added {selectedProduct.ProductName}({selectedProduct.Type}) to your favorites." };
        }
    }

}
