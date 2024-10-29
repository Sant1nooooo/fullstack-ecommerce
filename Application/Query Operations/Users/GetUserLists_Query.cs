using MediatR;
using server.Application.Interfaces;
using server.Application.Models;

namespace server.Application.Query_Operations.Users
{
    public class GetUserLists_Query : IRequest<IEnumerable<User>?>;

    public class GetUserLists_Handler : IRequestHandler<GetUserLists_Query, IEnumerable<User>?>
    {
        private readonly IUsersRepository _userRepository;
        public GetUserLists_Handler(IUsersRepository usersRepository)
        {
            _userRepository = usersRepository;
        }
        public async Task<IEnumerable<User>?> Handle(GetUserLists_Query request, CancellationToken ct)
        {
            //IEnumerable<User> userLists = await _context.User.ToListAsync(cancellationToken: ct);
            IEnumerable<User> userLists = await _userRepository.GetUserListsAsync();
            if (userLists.Count() == 0) return null;
            return userLists;
        }
    }
}
