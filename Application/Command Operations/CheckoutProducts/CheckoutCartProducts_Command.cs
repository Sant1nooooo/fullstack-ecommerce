using MediatR;
using server.Application.Interfaces;
using server.Application.Models;
using static server.Core.ResponseModels;

namespace server.Application.Command_Operations.CheckoutProducts
{
    public class CheckoutCartProducts_Command : IRequest<CheckoutCartProductsResult>
    {
        public List<int>? CartProductIDList { get; set; }
        public int CustomerID { get; set; }
    }
    public class CheckoutCartProducts_CommandHandler : IRequestHandler<CheckoutCartProducts_Command, CheckoutCartProductsResult>
    {
        private readonly ICustomerRepository _customer;
        public CheckoutCartProducts_CommandHandler(ICustomerRepository customer) 
        {
            _customer = customer;
        }
        public async Task<CheckoutCartProductsResult> Handle(CheckoutCartProducts_Command request, CancellationToken ct)
        {
            IEnumerable<CartProducts>? list = await _customer.CheckoutCartProductAsync(request.CartProductIDList!, request.CustomerID);//[9,13] , 2
            //Checking nalang ng product hindi nasama.

            if(list is null) return new CheckoutCartProductsResult() { IsCheckedOut = false, Message = "WARNING: There's a product that is already paid!" };
            
            return new CheckoutCartProductsResult() { IsCheckedOut = true, ProductList = list };
        }
    }
}
