using MediatR;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using server.Application.Interfaces;
using server.Application.Models;
using static server.Core.ResponseModels;

namespace server.Application.Query_Operations.FavoriteProduct
{
    public class GetFavoriteProducts_Query : IRequest<GetFavoriteProduct_Result>
    {
        public int CustomerID { get; set; }
    }
    public class GetFavoriteProducts_QueryHandler : IRequestHandler<GetFavoriteProducts_Query, GetFavoriteProduct_Result>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IFavoriteProductRepository _favoriteRepository;
        public GetFavoriteProducts_QueryHandler(IUsersRepository usersRepository, IFavoriteProductRepository favoriteRepository)
        {
            _usersRepository = usersRepository;
            _favoriteRepository = favoriteRepository;
        }

        public async Task<GetFavoriteProduct_Result> Handle(GetFavoriteProducts_Query request, CancellationToken ct)
        {
            Customer? selectedCustomer = await _usersRepository.GetCustomerAsync(request.CustomerID);
            if (selectedCustomer is null) return new GetFavoriteProduct_Result() { IsSuccessful = false, Message = "WARNING: Invalid customerID!" };

            IEnumerable<Product?> favoriteProducts = await _favoriteRepository.GetFavoriteProductAsync(request.CustomerID);

            if (favoriteProducts.Count() < 1)
            {
                return new GetFavoriteProduct_Result() { 
                    IsSuccessful = false, 
                    Message = "WARNING: No favorites added yet. Browse through our products and tap the heart icon to add your favorites here!" 
                };
            }

            return new GetFavoriteProduct_Result() { IsSuccessful = true, FavoriteProductList = favoriteProducts!};

        }
    }
}
