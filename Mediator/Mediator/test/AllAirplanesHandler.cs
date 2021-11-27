using System.Threading.Tasks;

namespace Mediator.test
{
    public class AllAirplanesHandler : IRequestHandler<GetAllAirplanes, AllAirplanesResponse>
    {
        public Task<AllAirplanesResponse> Handle(GetAllAirplanes request)
        {
            return Task.FromResult(new AllAirplanesResponse());
        }
    }
}
