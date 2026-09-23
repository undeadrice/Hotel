using MediatR;

namespace Hotel.SharedPipeline;

public interface ICommand : IRequest
{
}

public interface ICommand<TResponse> : IRequest<TResponse>
{
}
