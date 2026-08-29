using Hotel.Application.Auth.Dtos;
using Hotel.Application.Auth.Services;
using MediatR;

namespace Hotel.Application.Auth.Commands;

public record LoginCommand(string Email, string Password) : IRequest<TokenDto>;

public class LoginCommandHandler(IAuthService authService) : IRequestHandler<LoginCommand, TokenDto>
{
    public async Task<TokenDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var token = await authService.Login(request.Email, request.Password);

        return new TokenDto(token, DateTime.Now.AddHours(1));
    }
}