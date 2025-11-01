using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Economy.Domain.Enum;
using Economy.Domain.Exceptions;
using Economy.Domain.IManager;
using Economy.Domain.Models;

namespace Economy.Domain
{
    public class FlightManager : IFlightManager
    {
        public Action<Icao> LocationUpdated { get; set; }
        public Action<AircraftSpecs> AircraftUpdated { get; set; }
        public Action<SimStatus> StatusUpdated { get; set; }
        public Action<EconomyStatus> EconomyStatusUpdated { get; set; }
        public Action<PlayerFlightStatus> FlightStatusUpdated { get; set; }

        private readonly ISimConnectManager _simConnectManager;
        private readonly IEconomyClient _economyClient;
        private readonly IIcaoManager _icaoManager;

        private EconomyStatus _currentEconomyStatus = EconomyStatus.Disconnected;
        private SimStatus _currentSimStatus = SimStatus.Disconnected;

        private Icao _currentLocation;
        private AircraftSpecs _currentAircraft;
        private Player _currentPlayer;

        public FlightManager(ISimConnectManager simConnectManager, IEconomyClient economyClient, IIcaoManager icaoManager)
        {
            _simConnectManager = simConnectManager;
            _economyClient = economyClient;
            _icaoManager = icaoManager;

            _simConnectManager.AircraftDataUpdated += AircraftDataUpdated;
            _simConnectManager.FlightDataUpdated += FlightDataUpdated;
            _simConnectManager.StatusUpdated += StatusUpdate;

            //InitPlayer();
        }

        // gets player info from server
        public async Task<Player> GetPlayer()
        {
            return await _economyClient.GetPlayer();
        }

        public async Task<User> Login(string userName, string password)
        {
            try
            {
                var user = await _economyClient.Authenticate(userName, password);
                _currentPlayer = await _economyClient.GetPlayer();
                _currentEconomyStatus = EconomyStatus.Connected;
                EconomyStatusUpdated.Invoke(EconomyStatus.Connected);
                AircraftDataUpdated(_currentAircraft);
                return user;
            }
            catch (ClientUnauthorizedException ex)
            {
                EconomyStatusUpdated.Invoke(EconomyStatus.Disconnected);
            }
            catch (ClientBadRequestException ex)
            {
                EconomyStatusUpdated.Invoke(EconomyStatus.Disconnected);
            }
            catch (ClientNotFoundException ex)
            {
                EconomyStatusUpdated.Invoke(EconomyStatus.Disconnected);
            }

            return null;
        }

        public async Task<Player> StartFlight()
        {
            if (_currentEconomyStatus == EconomyStatus.Disconnected)
                return null;

            try
            {
                _currentPlayer = await _economyClient.StartFlight(_currentAircraft.Latitude, _currentAircraft.Longitude,_currentAircraft.Title);
                FlightStatusUpdated.Invoke(_currentPlayer.FlightStatus);
                return _currentPlayer;
            }
            catch (ClientUnauthorizedException ex)
            {
                EconomyStatusUpdated.Invoke(EconomyStatus.Disconnected);
            }
            catch (ClientBadRequestException ex)
            {
                EconomyStatusUpdated.Invoke(EconomyStatus.Disconnected);
            }
            catch (ClientNotFoundException ex)
            {
                EconomyStatusUpdated.Invoke(EconomyStatus.Disconnected);
            }

            return null;
        }

        public async Task<Player> EndFlight()
        {
            if (_currentEconomyStatus == EconomyStatus.Disconnected)
                return null;

            try
            {
                _currentPlayer = await _economyClient.EndFlight(_currentAircraft.Latitude, _currentAircraft.Longitude, _currentAircraft.Title);
                FlightStatusUpdated.Invoke(_currentPlayer.FlightStatus);
                return _currentPlayer;
            }
            catch (ClientUnauthorizedException ex)
            {
                EconomyStatusUpdated.Invoke(EconomyStatus.Disconnected);
            }
            catch (ClientBadRequestException ex)
            {
                EconomyStatusUpdated.Invoke(EconomyStatus.Disconnected);
            }
            catch (ClientNotFoundException ex)
            {
                EconomyStatusUpdated.Invoke(EconomyStatus.Disconnected);
            }

            return null;
        }

        private async void FlightDataUpdated(FlightData obj)
        {
            if (_currentEconomyStatus == EconomyStatus.Disconnected)
                return;

            // do stuff with flight data (location, plane telemetry, etc)
            var location = _icaoManager.GetCurrentLocation(obj.Latitude, obj.Longitude);

            if (_currentLocation == null || 
                (_currentLocation != null && location != null && _currentLocation.ICAO != location.ICAO))
            {
                _currentLocation = location;
                LocationUpdated.Invoke(_currentLocation);
            }
        }

        // aircraft changed
        private async void AircraftDataUpdated(AircraftSpecs obj)
        {
            _currentAircraft = obj;

            if (_currentEconomyStatus == EconomyStatus.Disconnected)
                return;

            // do stuff when a plane changed (load fuel, cargo)
            var playerFlight = await _economyClient.GetPlayerFlight(obj.Latitude, obj.Longitude, obj.Title);
            obj.IsPlaneCompatible = playerFlight.CurrentAircraftIsCompatible;
            obj.IsRented = playerFlight.CurrentLocationHasRentedAircraft;
            _currentAircraft = obj;
            AircraftUpdated.Invoke(obj);
        }

        private void StatusUpdate(SimStatus obj)
        {
            _currentSimStatus = obj;
            StatusUpdated.Invoke(obj);
        }
    }
}
