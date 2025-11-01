using System.Collections.Generic;
using API.Domain.Models;

namespace API.Domain.IManager
{
    public interface IAircraftManager
    {
        List<AircraftDM> GetAircraftByICAO(string icao);
    }
}