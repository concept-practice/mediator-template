using System.Collections.Generic;

namespace Mediator.test
{
    public class AllAirplanesResponse
    {
        public AllAirplanesResponse()
        {
            Airplanes = new List<Airplane>
            {
                new Airplane(),
                new Airplane(),
                new Airplane(),
            };
        }

        public IEnumerable<Airplane> Airplanes { get; }
    }
}