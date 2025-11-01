using System;
using System.Threading.Tasks;
using Economy.Domain.Enum;
using Economy.Domain.Models;

namespace Economy.Domain
{
    public interface IFlightManager
    {
        Action<Icao> LocationUpdated { get; set; }
        Action<SimStatus> StatusUpdated { get; set; }
        Action<AircraftSpecs> AircraftUpdated { get; set; }
        Action<EconomyStatus> EconomyStatusUpdated { get; set; }
        Action<PlayerFlightStatus> FlightStatusUpdated { get; set; }
        Task<User> Login(string userName, string password);
        Task<Player> StartFlight();
        Task<Player> EndFlight();
    }
}