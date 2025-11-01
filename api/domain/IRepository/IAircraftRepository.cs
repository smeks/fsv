using System.Collections.Generic;
using API.Domain.Models;

namespace API.Domain.IRepository
{
    public interface IAircraftRepository
    {
        List<AircraftDM> GetAircraftByICAO(string icao);
        AircraftDM Update(AircraftDM aircraft);
        AircraftDM GetAircraft(string id);
    }
}