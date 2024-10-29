using MediatR;
using server.Application.Interfaces;
using server.Application.Models;
using server.Infrastructure.Auth;
using static server.Core.ResponseModels;

namespace server.Application.Query_Operations.Users
{
    public class Login_Query : IRequest<LoginUser_Result>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
    public class Login_QueryHandler : IRequestHandler<Login_Query, LoginUser_Result>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly TokenGenerator _tokenGenerator;
        public Login_QueryHandler(IUsersRepository usersRepository, TokenGenerator tokenGenerator)
        {
            _usersRepository = usersRepository;
            _tokenGenerator = tokenGenerator;
        }
        public async Task<LoginUser_Result> Handle(Login_Query request, CancellationToken ct)
        {
            User? user = await _usersRepository.LoginUserAsync(request.Email!);

            //Invalid Email
            if (user == null)
            {
                return new LoginUser_Result()
                {
                    IsInvalid = true,
                    ErrorMessage = "WARNING: Email does not exists!"
                };
            }

            //Invalid Password
            if (user.Password != request.Password)
            {
                return new LoginUser_Result()
                {
                    IsInvalid = true,
                    ErrorMessage = "WARNING: Password is incorrect!"
                };
            }
            string generatedToken = _tokenGenerator.CreateToken(user);
            return new LoginUser_Result() { IsInvalid = false, Token = generatedToken};
        }
    }
}
