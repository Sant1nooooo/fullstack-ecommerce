using MediatR;

using server.Application.Interfaces;
using server.Application.Models;

namespace server.Application.Query_Operations.Products
{
    public class GetProductList_Query : IRequest<IEnumerable<ProductSubImages>?>;

    public class GetProductList_QueryHandler : IRequestHandler<GetProductList_Query, IEnumerable<ProductSubImages>?>
    {
        private readonly IProductRepository _productRepository;
        public GetProductList_QueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<IEnumerable<ProductSubImages>?> Handle(GetProductList_Query request, CancellationToken ct)
        {
            //This include the subimages.
            IEnumerable<ProductSubImages>? productList = await _productRepository.GetProductListAsync();
            return productList;
        }
    }
}
