using Hotel.Auth.Application.Auth.Services;
using MediatR;

namespace Hotel.Auth.Application.Auth.Commands;

public record LoginCommand(string Email, string Password) : IRequest<string>;

internal class LoginCommandHandler(IAuthService authService) : IRequestHandler<LoginCommand, string>
{
    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await authService.Login(request.Email, request.Password);
    }
}