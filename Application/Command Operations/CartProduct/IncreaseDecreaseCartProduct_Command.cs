using MediatR;
using server.Application.Interfaces;
using server.Application.Models;
using static server.Core.ResponseModels;
namespace server.Application.Command_Operations.CartProduct
{
    public class IncreaseDecreaseCartProduct_Command : IRequest<IncreaseDecreaseQuantity_Result>
    {
        public int CustomerID { get; set; }
        public int CartProductID { get; set; }
        public string? Operation {  get; set; }

    }
    public class IncreaseCartProduct_CommandHandler : IRequestHandler<IncreaseDecreaseCartProduct_Command, IncreaseDecreaseQuantity_Result>
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

        public async Task<IncreaseDecreaseQuantity_Result> Handle(IncreaseDecreaseCartProduct_Command request, CancellationToken ct)
        {
            Product? selectedProduct = await _productRepository.GetProductAsync(request.CartProductID);
            Customer? selectedCustomer = await _usersRepository.GetCustomerAsync(request.CustomerID);
            CartProducts? selectedCartProduct = await _customerRepository.GetCartProductsAsync(request.CartProductID, request.CustomerID);

            if (selectedProduct is null) return new IncreaseDecreaseQuantity_Result() { IsSuccessful = false, Message = $"WARNING: ProductID does not exist!" };
            if (selectedCustomer is null) return new IncreaseDecreaseQuantity_Result() { IsSuccessful = false, Message = $"WARNING: CustomerID does not exist!" };
            if (selectedCartProduct == null) return new IncreaseDecreaseQuantity_Result() { IsSuccessful = false, Message = $"WARNING: ({selectedProduct.ProductName}) does not exist in your cart!" };

            switch (request.Operation!.ToLower())
            {
                case "increase":
                    if ((selectedCartProduct.Quantity + 1) > selectedProduct.AvailableQuantity)
                    {
                        return new IncreaseDecreaseQuantity_Result() { IsSuccessful = false, Message = $"WARNING: Quantity will exceed the product's available quantity!" };
                    }
                    selectedCartProduct.Quantity += 1;
                    break;

                case "decrease":
                    if((selectedCartProduct.Quantity - 1) < 1)
                    {
                        return new IncreaseDecreaseQuantity_Result() { IsSuccessful = false, Message = $"WARNING: Quantity cannot be less than 1!" };
                    }
                    selectedCartProduct.Quantity -= 1;
                    break;

                default:
                    return new IncreaseDecreaseQuantity_Result() { IsSuccessful = true, Message = "WARNING: Invalid operation!" };
            }

            await _customerRepository.UpdateChangesAsync();
            return new IncreaseDecreaseQuantity_Result() { IsSuccessful = true, Message = $"({selectedCartProduct.Product!.ProductName}) quantity updated successfully." };
        }
    }
}
