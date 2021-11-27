using System;
using System.Reflection;
using System.Threading.Tasks;
using Mediator.test;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var collection = new ServiceCollection();

            collection.RegisterHandlers(Assembly.GetExecutingAssembly());
            collection.AddTransient<IMediator, Mediator>();

            var provider = collection.BuildServiceProvider();

            var mediator = provider.GetService<IMediator>();

            var result = await mediator.Send(new GetAllAirplanes());

            foreach (var plane in result.Airplanes)
            {
                Console.WriteLine(plane.Id);
            }

            await mediator.Publish(new AirplaneCreated());

            Console.ReadLine();
        }
    }
}
