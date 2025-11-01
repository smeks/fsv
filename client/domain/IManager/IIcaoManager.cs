using Economy.Domain.Models;

namespace Economy.Domain.IManager
{
    public interface IIcaoManager
    {
        Icao GetCurrentLocation(double lat, double lon);
        Icao GetIcaoByIcao(string icao);
    }
}