using System;
using System.Threading.Tasks;

namespace Mediator.test
{
    public class AirplaneCreatedHandler : INotificationHandler<AirplaneCreated>
    {
        public Task Handle(AirplaneCreated notification)
        {
            Console.WriteLine(notification.DateTime);

            return Task.CompletedTask;
        }
    }
}
