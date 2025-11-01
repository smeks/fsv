using API.Data.Models;
using API.Domain.Models;
using AutoMapper;

namespace API.Data.Mappers
{
    public class JobRepositoryMappingProfile : Profile
    {
        public JobRepositoryMappingProfile()
        {
            CreateMap<Job, JobDM>()
                .ReverseMap();
        }
    }
}
