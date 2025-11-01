using AutoMapper;
using Economy.Client.DTO;
using Economy.Domain.Models;

namespace Economy.Client
{
    public class EconomyClientMappingProfile : Profile
    {
        public EconomyClientMappingProfile()
        {
            CreateMap<AircraftDC, Aircraft>();
            CreateMap<JobDC, Job>();
            CreateMap<PlayerDC, Player>();
            CreateMap<UserDC, User>();
            CreateMap<PlayerFlightDC, PlayerFlight>();
        }
    }
}
