using MediatR;
using server.Application.Interfaces;
using server.Application.Models;

namespace server.Application.Query_Operations.Users
{
    public class GetCustomer_Query : IRequest<Customer?>
    {
        public int Id { get; set; }
    }

    public class GetCustomer_QueryHandler : IRequestHandler<GetCustomer_Query, Customer?>
    {
        private readonly IUsersRepository _userRepository;
        public GetCustomer_QueryHandler(IUsersRepository usersRepository)
        {
            _userRepository = usersRepository;
        }
        public async Task<Customer?> Handle(GetCustomer_Query request, CancellationToken ct)
        {
            //Customer? searchedCustomer = await _context.User.OfType<Customer>().FirstOrDefaultAsync(eachCustomer => eachCustomer.ID == request.Id, cancellationToken: ct);
            Customer? searchedCustomer = await _userRepository.GetCustomerAsync(request.Id);

            if (searchedCustomer == null) return null;

            return searchedCustomer;
        }
    }
}
