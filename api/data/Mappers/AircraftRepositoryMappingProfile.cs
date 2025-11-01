using API.Data.Models;
using API.Domain.Models;
using AutoMapper;

namespace API.Data.Mappers
{
    public class AircraftRepositoryMappingProfile : Profile
    {
        public AircraftRepositoryMappingProfile()
        {
            CreateMap<Aircraft, AircraftDM>()
                .ReverseMap();
        }
    }
}
