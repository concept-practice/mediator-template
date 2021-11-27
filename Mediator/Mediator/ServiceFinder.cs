using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator
{
    public static class ServiceFinder
    {
        public static void RegisterHandlers(this IServiceCollection collection, Assembly assembly)
        {
            var types = assembly.GetTypes().ToList();

            var requests = types
                .Where(x => !x.IsGenericType && !x.IsAbstract && x.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>))).ToList();

            var notifications = types
                .Where(x => !x.IsGenericType && !x.IsAbstract && x.GetInterfaces()
                    .Any(i => i == typeof(INotification))).ToList();

            var requestHandlers = types
                .Where(x => !x.IsGenericType && x.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))).ToList();

            var notificationHandlers = types
                .Where(x => !x.IsGenericType && x.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>))).ToList();

            foreach (var type in requests)
            {
                var responseType = type.GetInterfaces()[0].GetGenericArguments()[0];

                var handlerType = requestHandlers.FirstOrDefault(y =>
                    {
                        var typeInterface = y.GetInterfaces()[0];

                        var genericArguments = typeInterface.GetGenericArguments();

                        if (type == genericArguments[0] && responseType == genericArguments[1])
                        {
                            return true;
                        }

                        return false;
                    });

                if (handlerType != null)
                {
                    collection.AddTransient(typeof(IRequestHandler<,>).MakeGenericType(type, responseType), handlerType);
                }
            }

            foreach (var notification in notifications)
            {
                var handlerType = notificationHandlers.FirstOrDefault(y =>
                    {
                        var typeInterface = y.GetInterfaces()[0];

                        var genericArguments = typeInterface.GetGenericArguments();

                        if (notification == genericArguments[0])
                        {
                            return true;
                        }

                        return false;
                    });

                if (handlerType != null)
                {
                    collection.AddTransient(typeof(INotificationHandler<>).MakeGenericType(notification), handlerType);
                }
            }
        }
    }
}
