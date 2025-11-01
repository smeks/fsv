using System.Threading.Tasks;
using Economy.Domain.Models;

namespace Economy.Domain.IManager
{
    public interface IEconomyClient
    {
        Task<Player> GetPlayer();
        Task<Player> StartFlight(double lat, double lon, string model);
        Task<Player> EndFlight(double lat, double lon, string model);
        Task<PlayerFlight> GetPlayerFlight(double lat, double lon, string model);
        Task<User> Authenticate(string userName, string password);
    }
}