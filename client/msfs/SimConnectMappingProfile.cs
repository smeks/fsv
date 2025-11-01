using AutoMapper;
using Economy.Domain.Models;
using Economy.FSX.Models;

namespace Economy.FSX
{
    public class SimConnectMappingProfile : Profile
    {
        public SimConnectMappingProfile()
        {
            CreateMap<AircraftSpecs, SimConnectAircraftSpecs>()
                .ReverseMap();
            CreateMap<FlightData, SimConnectFlightData>()
                .ReverseMap();
        }
    }
}
