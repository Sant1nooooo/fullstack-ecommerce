using MediatR;
using server.Application.Interfaces;
using server.Application.Models;

namespace server.Application.Command_Operations
{
    public class CreateAdmin_Command : IRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class CreateAdmin_CommandHandler : IRequestHandler<CreateAdmin_Command>
    {
        private readonly IUsersRepository _usersRepository;
        public CreateAdmin_CommandHandler(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        public async Task Handle(CreateAdmin_Command request, CancellationToken ct)
        {
            User newAdmin = new User(request.FirstName!, request.LastName!, request.Email!, request.Password!, "Admin");
            await _usersRepository.CreateAdminAsync(newAdmin);
        }
    }
}
