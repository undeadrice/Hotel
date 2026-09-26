using Hotel.Auth.Application.Auth.Dtos;

namespace Hotel.Auth.Application.Auth.Services;

public interface IAuthService
{
    Task<TokenDto> Login(string email, string password);
}