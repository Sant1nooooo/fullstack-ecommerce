using FluentValidation;
using server.Application.Command_Operations.Products;
using server.Application.Interfaces;

namespace server.Application.Validators
{
    public class CreateProduct_Validator : AbstractValidator<CreateProduct_Command>
    {
        public CreateProduct_Validator(IProductRepository _productRepository)
        {
            RuleFor(product => product.ProductName)
                .MustAsync(async (ProductName, _) =>
                {
                    return !await _productRepository.IsProductNameExistingAsync(ProductName!);
                })
                .WithMessage("WARNING: Product Name is already existing!");
        }
    }
}
