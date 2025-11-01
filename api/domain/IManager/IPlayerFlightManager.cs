using API.Domain.Models;

namespace API.Domain.IManager
{
    public interface IPlayerFlightManager
    {
        PlayerFlightDM GetPlayerFlight(string userName, double lat, double lon, string model);
        PlayerDM StartFlight(string userName, double lat, double lon, string model);
        PlayerDM EndFlight(string userName, double lat, double lon, string model);
    }
}