using System.Threading.Tasks;

namespace Mediator
{
    public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public Task<TResponse> Handle(TRequest request);
    }

    public interface IRequestHandler<in TRequest> : IRequestHandler<TRequest, Unit> where TRequest : IRequest<Unit>
    {
    }
}
