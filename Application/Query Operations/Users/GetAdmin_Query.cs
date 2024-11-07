using server.Application.Models;
using server.Application.Interfaces;
using MediatR;
using static server.Core.ResponseModels;
namespace server.Application.Query_Operations.Users
{
    public class GetAdmin_Query : IRequest<GetAdmin_Result>
    {
        public int Id { get; set; }
    }
    public class GetAdmin_QueryHandler : IRequestHandler<GetAdmin_Query, GetAdmin_Result>
    {
        private readonly IUsersRepository _userRepository;
        public GetAdmin_QueryHandler(IUsersRepository usersRepository)
        {
            _userRepository = usersRepository;
        }
        public async Task<GetAdmin_Result> Handle(GetAdmin_Query request, CancellationToken ct)
        {
            User? searchedAdmin =  _userRepository.NewGetAdminAsync(request.Id);
            Console.WriteLine(searchedAdmin!.FirstName);

            if (searchedAdmin.FirstName == null) return new GetAdmin_Result() { IsRetrieved = false, Message = "WARNING: Invalid adminID!"};
            
            return new GetAdmin_Result() { IsRetrieved = true, User = searchedAdmin};
        }

    }
}


