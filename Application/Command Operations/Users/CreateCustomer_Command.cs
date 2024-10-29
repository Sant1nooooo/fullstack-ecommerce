using FluentValidation;
using MediatR;
using server.Application.Interfaces;
using server.Application.Models;
using static server.Core.ResponseModels;

namespace server.Application.Command_Operations
{
    public class CreateCustomer_Command : IRequest<CreateCustomer_Result>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
    public class CreateCustomer_CommandHandler : IRequestHandler<CreateCustomer_Command, CreateCustomer_Result>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IValidator<CreateCustomer_Command> _validator;
        public CreateCustomer_CommandHandler(IUsersRepository usersRepository, IValidator<CreateCustomer_Command> validator)
        {
            _usersRepository = usersRepository;
            _validator = validator;
        }
        public async Task<CreateCustomer_Result> Handle(CreateCustomer_Command request, CancellationToken ct)
        {
            //Validating the input data specifically the `Email` attribute that must be unique.
            var result = await _validator.ValidateAsync(request);
            if (!result.IsValid)
            {
                return new CreateCustomer_Result() 
                {
                    IsExisting = true, 
                    Message = string.Join(", ", result.Errors.Select(e => e.ErrorMessage))
                };
            }

            Customer customer = new Customer(request.FirstName!, request.LastName!, request.Email!, request.Password!);
            await _usersRepository.CreateCustomerAsync(customer);

            return new CreateCustomer_Result() { IsExisting = false, Message = "Account(Customer) created successfully!" };
        }
    }
}
