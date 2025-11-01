using API.Domain.IManager;
using API.Domain.IRepository;
using API.Domain.Models;

namespace API.Domain.Managers
{
    public class PlayerManager : IPlayerManager
    {
        private readonly IJobRepository _jobRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IAircraftRepository _aircraftRepository;

        public PlayerManager(IJobRepository jobRepository, IPlayerRepository playerRepository, IAircraftRepository aircraftRepository)
        {
            _jobRepository = jobRepository;
            _playerRepository = playerRepository;
            _aircraftRepository = aircraftRepository;
        }

        public PlayerDM GetPlayer(string userName)
        {
            return _playerRepository.Get(userName);
        }

        public PlayerDM CreatePlayer(string userName)
        {
            return _playerRepository.Create(userName);
        }

        public PlayerDM AddJob(string userName, string jobId)
        {
            var job = _jobRepository.GetJobById(jobId);
            var playerDM = _playerRepository.Get(userName);

            playerDM.AddJob(job);

            _playerRepository.Update(userName, playerDM);

            return playerDM;
        }

        public PlayerDM RemoveJob(string userName, string jobId)
        {
            var playerDM = _playerRepository.Get(userName);
  
            playerDM.RemoveJob(jobId);

            _playerRepository.Update(userName, playerDM);
            
            return playerDM;
        }

        public PlayerDM RentAircraft(string userName, string aircraftId)
        {
            var player = GetPlayer(userName);
            var aircraft = _aircraftRepository.GetAircraft(aircraftId);

            player.RentAircraft(aircraft);

            return _playerRepository.Update(userName, player);
        }

        public PlayerDM ReleaseRentedAircraft(string userName)
        {
            var player = GetPlayer(userName);

            player.ReleaseRentedAircraft();

            return _playerRepository.Update(userName, player);
        }
    }
}
