using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mediator
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _provider;

        public Mediator(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            var requestType = request.GetType();

            var responseType = requestType.GetInterfaces()[0].GetGenericArguments()[0];

            var service = (IWrapper<TResponse>)Activator.CreateInstance(typeof(Wrapper<,>).MakeGenericType(requestType, responseType));

            return await service.Execute(request, _provider);
        }

        public async Task Publish<TNotification>(TNotification notification) where TNotification : INotification
        {
            var services = _provider.GetServices<INotificationHandler<TNotification>>();

            if (!services.Any())
            {
                throw new NullReferenceException("No handlers found. Did you forget to register?");
            }

            foreach (var service in services)
            {
                await service.Handle(notification);
            }
        }

        private interface IWrapper<TResponse>
        {
            Task<TResponse> Execute(IRequest<TResponse> request, IServiceProvider provider);
        }

        private class Wrapper<TRequest, TResponse> : IWrapper<TResponse> where TRequest : IRequest<TResponse>
        {
            public async Task<TResponse> Execute(IRequest<TResponse> request, IServiceProvider provider)
            {
                return await provider.GetService<IRequestHandler<TRequest, TResponse>>().Handle((TRequest)request);
            }
        }
    }
}
