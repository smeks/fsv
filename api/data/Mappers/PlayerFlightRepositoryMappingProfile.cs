using API.Data.Models;
using API.Domain.Models;
using AutoMapper;

namespace API.Data.Mappers
{
    public class PlayerFlightRepositoryMappingProfile : Profile
    {
        public PlayerFlightRepositoryMappingProfile()
        {
            CreateMap<Job, JobDM>()
                .ReverseMap();

            CreateMap<Player, PlayerDM>()
                .ReverseMap();

            CreateMap<Aircraft, AircraftDM>()
                .ReverseMap();
        }
    }
}
