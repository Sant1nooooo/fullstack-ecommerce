using Azure.Core;
using FluentValidation;
using server.Application.Command_Operations.CartProduct;
using server.Application.Interfaces;
using server.Application.Models;
using server.Application.Repositories;

namespace server.Application.Validators
{
    public class RemoveCartProduct_Validator:AbstractValidator<RemoveCartProduct_Command>
    {

        public RemoveCartProduct_Validator(IUsersRepository _usersRepository, IProductRepository _productRepository)
        {
            RuleFor(customer => customer.CustomerID)
            .MustAsync(async (CustomerID, _) => {
                Customer? selectedCustomer = await _usersRepository.GetCustomerAsync(CustomerID);
                if (selectedCustomer is null) return false;
                return true;
            })
            .WithMessage("WARNING: CustomerID does not exist! asd");

            RuleFor( product => product.CartProductID)
                .MustAsync(async (CartProductID, _) => {
                    Product? selectedProduct = await _productRepository.GetProductAsync(CartProductID);
                    if (selectedProduct is null) return false;
                    return true;
                })
                .WithMessage("WARNING: CartProductID does not exist! asd");
        }
    }
}
