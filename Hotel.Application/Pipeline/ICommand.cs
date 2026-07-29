using MediatR;

namespace Hotel.Application.Pipeline;

public interface ICommand : IRequest
{
}

public interface ICommand<TResponse> : IRequest<TResponse>
{
}

