using MediatR;
using server.Application.Interfaces;
using server.Application.Models;
using static server.Core.ResponseModels;
namespace server.Application.Query_Operations.CartProduct
{
    public class GetCartProducts_Query : IRequest<GetCartProductList_Result>
    {
        public int CustomerID { get; set; }
    }

    public class GetCartProducts_QueryHandler : IRequestHandler<GetCartProducts_Query, GetCartProductList_Result>
    {
        private readonly ICustomerRepository _customerRepository;
        public GetCartProducts_QueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        
        public async Task<GetCartProductList_Result> Handle(GetCartProducts_Query request, CancellationToken ct)
        {
            IEnumerable<CartProducts>? result = await _customerRepository.GetCartProductListAsync(request.CustomerID);
            if(result!.Count() < 1) return new GetCartProductList_Result() { IsSuccessful = false, Message = "WARNING: You don't have any products on your cart!"};

            return new GetCartProductList_Result() { IsSuccessful = true, CartProductList = result};
        }
    }
}
