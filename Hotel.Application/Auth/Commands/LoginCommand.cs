using Hotel.Application.Auth.Dtos;
using MediatR;

namespace Hotel.Application.Auth.Commands;

public record LoginCommand(string Email, string Password) : IRequest<TokenDto>;