using server.Application.Models;
using server.Application.Interfaces;
using MediatR;
namespace server.Application.Query_Operations.Users
{
    public class GetAdmin_Query : IRequest<User?>
    {
        public int Id { get; set; }
    }
    public class GetAdmin_QueryHandler : IRequestHandler<GetAdmin_Query, User?>
    {
        private readonly IUsersRepository _userRepository;
        public GetAdmin_QueryHandler(IUsersRepository usersRepository)
        {
            _userRepository = usersRepository;
        }
        public async Task<User?> Handle(GetAdmin_Query request, CancellationToken ct)
        {
            User? searchedAdmin = await _userRepository.GetAdminAsync(request.Id);
            Console.WriteLine(searchedAdmin!.FirstName);
            if (searchedAdmin == null || searchedAdmin!.Authentication.ToLower() != "admin") return null;
            return searchedAdmin;
        }

    }
}


