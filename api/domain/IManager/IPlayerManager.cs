using API.Domain.Models;

namespace API.Domain.IManager
{
    public interface IPlayerManager
    {
        PlayerDM GetPlayer(string userName);
        PlayerDM AddJob(string userName, string jobId);
        PlayerDM RemoveJob(string userName, string jobId);
        PlayerDM RentAircraft(string userName, string aircraftId);
        PlayerDM ReleaseRentedAircraft(string userName);
        PlayerDM CreatePlayer(string userName);
    }
}