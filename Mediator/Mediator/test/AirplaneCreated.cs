using System;

namespace Mediator.test
{
    public class AirplaneCreated : INotification
    {
        public DateTime DateTime { get; } = DateTime.Now;
    }
}
