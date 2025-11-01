using API.Domain.IManager;
using API.Domain.IRepository;
using API.Domain.Models;

namespace API.Domain.Managers
{
    public class PlayerFlightManager : IPlayerFlightManager
    {
        private readonly IPlayerFlightRepository _playerFlightRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IAircraftRepository _aircraftRepository;

        public PlayerFlightManager(IPlayerFlightRepository playerFlightRepository, IPlayerRepository playerRepository, IAircraftRepository aircraftRepository )
        {
            _playerFlightRepository = playerFlightRepository;
            _playerRepository = playerRepository;
            _aircraftRepository = aircraftRepository;
        }

        public PlayerFlightDM GetPlayerFlight(string userName, double lat, double lon, string model)
        {
            return _playerFlightRepository.GetPlayer(userName, lat, lon, model);
        }

        public PlayerDM StartFlight(string userName, double lat, double lon, string model)
        {
            var playerFlight = _playerFlightRepository.GetPlayer(userName, lat, lon, model);

            if (playerFlight.CurrentAircraftIsCompatible && playerFlight.CurrentLocationHasRentedAircraft)
            {
                playerFlight.Player.StartFlight();
            }

            _playerRepository.Update(userName, playerFlight.Player);

            return playerFlight.Player;
        }


        public PlayerDM EndFlight(string userName, double lat, double lon, string model)
        {
            var playerFlight = _playerFlightRepository.GetPlayer(userName, lat, lon, model);

            playerFlight.Player.EndFlight(playerFlight.CurrentLocation);

            _playerRepository.Update(userName, playerFlight.Player);

            return playerFlight.Player;
        }
    }
}
