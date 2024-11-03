using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using server.Application.Interfaces;
using server.Application.Models;
using static server.Core.ResponseModels;
namespace server.Application.Command_Operations.CartProduct
{
    public class RemoveCartProduct_Command : IRequest<RemoveCartProduct_Result>
    {
        public int CustomerID { get; set; }
        public int CartProductID { get; set; }
    }

    public class RemoveCartProduct_CommandHandler : IRequestHandler<RemoveCartProduct_Command, RemoveCartProduct_Result>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly ICustomerRepository _customerRepository;
        public RemoveCartProduct_CommandHandler(IProductRepository productRepository, IUsersRepository usersRepository, ICustomerRepository customerRepository)
        {
            _productRepository = productRepository;
            _usersRepository = usersRepository;
            _customerRepository = customerRepository;
        }
        public async Task<RemoveCartProduct_Result> Handle(RemoveCartProduct_Command request, CancellationToken ct)
        {
            //Validating if the `ProductID` and `CustomerID` is existing.
            Product? selectedProduct = await _productRepository.GetProductAsync(request.CartProductID);
            Customer? selectedCustomer = await _usersRepository.GetCustomerAsync(request.CustomerID);

            if (selectedProduct is null) return new RemoveCartProduct_Result() { IsDeleted = false, Message = $"WARNING: CartProductID does not exist!" };
            if (selectedCustomer is null) return new RemoveCartProduct_Result() { IsDeleted = false, Message = $"WARNING: CustomerID does not exist!" };

            CartProducts? selectedCartProduct = await _customerRepository.GetCartProductsAsync(request.CartProductID, request.CustomerID);

            if (selectedCartProduct == null) return new RemoveCartProduct_Result() { IsDeleted = false, Message = $"WARNING: ({selectedProduct.ProductName}) does not exist in your cart!"};

            await _customerRepository.RemoveCartProductAsync(selectedCartProduct);
            return new RemoveCartProduct_Result() { IsDeleted = true, Message = $"({selectedCartProduct.Product!.ProductName}) is successfully removed in your cart!" };
        }
    }
}
