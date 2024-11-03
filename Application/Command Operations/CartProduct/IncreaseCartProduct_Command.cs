using MediatR;
using server.Application.Interfaces;
using server.Application.Models;
using static server.Core.ResponseModels;
namespace server.Application.Command_Operations.CartProduct
{
    public class IncreaseCartProduct_Command : IRequest<IncreaseDecreaseQuantity_Result>
    {
        public int CustomerID { get; set; }
        public int CartProductID { get; set; }

    }
    public class IncreaseCartProduct_CommandHandler : IRequestHandler<IncreaseCartProduct_Command, IncreaseDecreaseQuantity_Result>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly ICustomerRepository _customerRepository;
        public IncreaseCartProduct_CommandHandler(IProductRepository productRepository, IUsersRepository usersRepository, ICustomerRepository customerRepository)
        {
            _productRepository = productRepository;
            _usersRepository = usersRepository;
            _customerRepository = customerRepository;
        }

        public async Task<IncreaseDecreaseQuantity_Result> Handle(IncreaseCartProduct_Command request, CancellationToken ct)
        {
            Product? selectedProduct = await _productRepository.GetProductAsync(request.CartProductID);
            Customer? selectedCustomer = await _usersRepository.GetCustomerAsync(request.CustomerID);
            CartProducts? selectedCartProduct = await _customerRepository.GetCartProductsAsync(request.CartProductID, request.CustomerID);

            if (selectedProduct is null) return new IncreaseDecreaseQuantity_Result() { IsSuccessful = false, Message = $"WARNING: ProductID does not exist!" };
            if (selectedCustomer is null) return new IncreaseDecreaseQuantity_Result() { IsSuccessful = false, Message = $"WARNING: CustomerID does not exist!" };
            if (selectedCartProduct == null) return new IncreaseDecreaseQuantity_Result() { IsSuccessful = false, Message = $"WARNING: ({selectedProduct.ProductName}) does not exist in your cart!" };

            if((selectedCartProduct.Quantity + 1) > selectedProduct.AvailableQuantity)
            {
                return new IncreaseDecreaseQuantity_Result() { IsSuccessful = false, Message = $"WARNINGL: Insufficient stock. Please reduce the quantity." };
            }
            selectedCartProduct.Quantity += 1;
            await _customerRepository.UpdateChangesAsync();
            return new IncreaseDecreaseQuantity_Result() { IsSuccessful = true, Message = $"({selectedCartProduct.Product!.ProductName}) quantity updated successfully." };
        }
    }
}
