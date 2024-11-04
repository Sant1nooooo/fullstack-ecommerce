using FluentValidation;
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
        private readonly IValidator<RemoveCartProduct_Command> _validator;
        public RemoveCartProduct_CommandHandler(IProductRepository productRepository, IUsersRepository usersRepository, ICustomerRepository customerRepository, IValidator<RemoveCartProduct_Command> validator)
        {
            _productRepository = productRepository;
            _usersRepository = usersRepository;
            _customerRepository = customerRepository;
            _validator = validator;
        }
        public async Task<RemoveCartProduct_Result> Handle(RemoveCartProduct_Command request, CancellationToken ct)
        {

            var result = await _validator.ValidateAsync(request);
            if (!result.IsValid)
            {
                return new RemoveCartProduct_Result()
                {
                    IsDeleted = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.ErrorMessage))
                };
            }

            CartProducts? selectedCartProduct = await _customerRepository.GetCartProductsAsync(request.CartProductID, request.CustomerID);

            if (selectedCartProduct is null) return new RemoveCartProduct_Result() { IsDeleted = false, Message = $"WARNING: Product does not exist in your cart!"};

            await _customerRepository.RemoveCartProductAsync(selectedCartProduct);
            return new RemoveCartProduct_Result() { IsDeleted = true, Message = $"({selectedCartProduct.Product!.ProductName}) is successfully removed in your cart!" };
        }
    }
}
