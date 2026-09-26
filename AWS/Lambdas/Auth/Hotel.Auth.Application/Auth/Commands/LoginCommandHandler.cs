using Hotel.Auth.Application.Auth.Dtos;
using Hotel.Auth.Application.Auth.Services;
using MediatR;

namespace Hotel.Auth.Application.Auth.Commands;

public record LoginCommand(string Email, string Password) : IRequest<TokenDto>;

internal class LoginCommandHandler(IAuthService authService) : IRequestHandler<LoginCommand, TokenDto>
{
    public async Task<TokenDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await authService.Login(request.Email, request.Password);
    }
}