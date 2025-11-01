using System.Collections.Generic;
using API.Domain.IManager;
using API.Domain.IRepository;
using API.Domain.Models;

namespace API.Domain.Managers
{
    public class AircraftManager : IAircraftManager
    {
        private readonly IAircraftRepository _aircraftRepository;

        public AircraftManager(IAircraftRepository aircraftRepository)
        {
            _aircraftRepository = aircraftRepository;
        }

        public List<AircraftDM> GetAircraftByICAO(string icao)
        {
            return _aircraftRepository.GetAircraftByICAO(icao);
        }
    }
}
