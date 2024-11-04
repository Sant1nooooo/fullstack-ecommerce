using MediatR;
using server.Application.Interfaces;
using server.Application.Models;
using static server.Core.ResponseModels;

namespace server.Application.Command_Operations.CartProduct
{
    public class AddCartProduct_Command : IRequest<AddCartProduct_Result>
    {
        public int CustomerID { get; set; }
        public int ProductID {  get; set; }
        public int Quantity { get; set; }
    }
    public class AddCartProduct_CommandHandler : IRequestHandler<AddCartProduct_Command, AddCartProduct_Result>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly ICustomerRepository _customerRepository;
        public AddCartProduct_CommandHandler(IProductRepository productRepository, IUsersRepository usersRepository, ICustomerRepository customerRepository)
        {
            _productRepository = productRepository;
            _usersRepository = usersRepository;
            _customerRepository = customerRepository;
        }
        public async Task<AddCartProduct_Result> Handle(AddCartProduct_Command request, CancellationToken ct)
        {
            Product? selectedProduct = await _productRepository.GetProductAsync(request.ProductID);
            Customer? selectedCustomer = await _usersRepository.GetCustomerAsync(request.CustomerID);

            if (selectedProduct is null ) return new AddCartProduct_Result() { IsFailure = true, Message = $"WARNING: ProductID does not exist!" };
            if (selectedCustomer is null) return new AddCartProduct_Result() { IsFailure = true, Message = $"WARNING: CustomerID does not exist!" };
            if(request.Quantity > selectedProduct.AvailableQuantity) return new AddCartProduct_Result() { IsFailure = true, Message = $"WARNING: Quantity will exceed the product's available quantity!" }; //For new items

            int price = 0;
            if(selectedProduct!.DiscountedPrice > 0)
            {
                price = request.Quantity * selectedProduct!.DiscountedPrice;
            }
            price = request.Quantity * selectedProduct!.OriginalPrice;
            

            //Checking if yung product naia-add to cart ni user is existing na.
            CartProducts? currentProduct = await _customerRepository.GetCartProductsAsync(request.ProductID, request.CustomerID);
            if(currentProduct != null)
            {
                if((currentProduct.Quantity + request.Quantity) > selectedProduct.AvailableQuantity)
                {
                    return new AddCartProduct_Result() { IsFailure = true, Message = $"WARNING: Quantity will exceed the product's available quantity!" };
                }

                //Then increase nalang yung quantity and price ng specific product sa cart ni specific customer.
                currentProduct.Price = currentProduct.Price + price;
                currentProduct.Quantity = currentProduct.Quantity + request.Quantity;
                await _customerRepository.UpdateChangesAsync();
                return new AddCartProduct_Result() { IsFailure = false, Message = $"Successfully added {request.Quantity} ({selectedProduct.ProductName})!" };
            }

            await _customerRepository.AddProductCartAsync(new CartProducts(selectedProduct, selectedCustomer!, price, request.Quantity));
            return new AddCartProduct_Result() { IsFailure = false, Message = $"({selectedProduct.ProductName}) successfully added to cart!"};
        }
    }
}
