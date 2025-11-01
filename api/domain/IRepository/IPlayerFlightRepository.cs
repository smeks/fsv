using API.Domain.Models;

namespace API.Domain.IRepository
{
    public interface IPlayerFlightRepository
    {
        PlayerFlightDM GetPlayer(string userName, double lat, double lon, string model);
    }
}