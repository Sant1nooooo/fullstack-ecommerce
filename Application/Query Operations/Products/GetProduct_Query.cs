using static server.Core.ResponseModels;
using MediatR;
using server.Application.Interfaces;
using server.Application.Models;

namespace server.Application.Query_Operations.Products
{
    public class GetProduct_Query : IRequest<GetProduct_Result>
    {
        public int ProductID { get; set; }
    }
    public class GetProduct_QueryHandler : IRequestHandler<GetProduct_Query,GetProduct_Result>
    {
        private readonly IProductRepository _productRepository;
        public GetProduct_QueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<GetProduct_Result> Handle(GetProduct_Query request, CancellationToken ct)
        {
            ProductSubImages? searchedProduct = await _productRepository.GetProductWithSubImagesAsync(request.ProductID);
            if (searchedProduct is null)
            {
                return new GetProduct_Result()
                {
                    IsNotExisting = true,
                    Message = "WARNING: Invalid productID!"
                };
            }
            return new GetProduct_Result() { IsNotExisting = false, Product = searchedProduct };
        }
    }
}
