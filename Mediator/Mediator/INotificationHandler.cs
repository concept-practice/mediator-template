using System.Threading.Tasks;

namespace Mediator
{
    public interface INotificationHandler<in TNotification> where TNotification : INotification
    {
        public Task Handle(TNotification notification);
    }
}
