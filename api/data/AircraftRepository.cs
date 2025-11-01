using System.Collections.Generic;
using System.Linq;
using API.Data.Mappers;
using API.Data.Models;
using API.Domain.Exceptions;
using API.Domain.IRepository;
using API.Domain.Models;
using AutoMapper;
using MongoDB.Driver;

namespace API.Data
{
    public class AircraftRepository : IAircraftRepository
    {
        private readonly IMongoCollection<Aircraft> _aircraftRepository;
        private readonly IMapper _mapper;

        public AircraftRepository()
        {
            _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<AircraftRepositoryMappingProfile>(); }).CreateMapper();
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("economy");
            _aircraftRepository = database.GetCollection<Aircraft>("aircrafts");
        }

        public List<AircraftDM> GetAircraftByICAO(string icao)
        {
            var aircrafts = _aircraftRepository.AsQueryable().Where(x => x.Location == icao);
            return _mapper.Map<List<AircraftDM>>(aircrafts);
        }

        public AircraftDM GetAircraft(string id)
        {
            var aircraft = _aircraftRepository.AsQueryable().SingleOrDefault(x => x.Id == id);
            return _mapper.Map<AircraftDM>(aircraft);
        }

        public AircraftDM Update(AircraftDM aircraft)
        {
            var entity = _mapper.Map<Aircraft>(aircraft);
            var updatedEntity = _aircraftRepository.FindOneAndReplace(x => x.Id == aircraft.Id, entity);

            if(updatedEntity == null)
                throw new RepositoryNotFoundException($"Could not find and update aircraft {aircraft.Id}");

            return GetAircraft(aircraft.Id);
        }
    }
}
