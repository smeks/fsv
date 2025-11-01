using API.Data.Models;
using API.Domain.Models;
using AutoMapper;

namespace API.Data.Mappers
{
    public class PlayerRepositoryMappingProfile : Profile
    {
        public PlayerRepositoryMappingProfile()
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
