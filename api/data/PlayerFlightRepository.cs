using System;
using System.Linq;
using API.Data.Mappers;
using API.Data.Models;
using API.Domain.Exceptions;
using API.Domain.IRepository;
using API.Domain.Models;
using AutoMapper;
using MongoDB.Driver;

namespace API.Data
{
    public class PlayerFlightRepository : IPlayerFlightRepository
    {
        private readonly IMongoCollection<Player> _playerCollection;
        private readonly IMongoCollection<CompatibleAircraft> _compatibleAircraftCollection;
        private readonly IMongoCollection<Airport> _icaoCollection;
        private readonly IMapper _mapper;

        public PlayerFlightRepository()
        {
            _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<PlayerFlightRepositoryMappingProfile>(); }).CreateMapper();
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("economy");
            _playerCollection = database.GetCollection<Player>("players");
            _compatibleAircraftCollection = database.GetCollection<CompatibleAircraft>("simaircrafts");
            _icaoCollection = database.GetCollection<Airport>("airports");
        }

        public PlayerFlightDM GetPlayer(string userName, double lat, double lon, string model)
        {
            var player = _playerCollection.AsQueryable().SingleOrDefault(x => x.UserName == userName);

            if(player == null)
                throw new RepositoryNotFoundException($"Player {userName} not found");

            var nearest = GetNearestAirport(lat, lon);
            var aircraftCompatible = IsAircraftCompatible(model);

            var nearestAirportHasRentedAircraft = nearest != null && player.RentedAircraft != null &&
                                                  nearest.Icao == player.RentedAircraft.Location;

            var playerDM = _mapper.Map<PlayerDM>(player);
            var playerFlight = new PlayerFlightDM()
            {
                Player = playerDM,
                CurrentAircraft = aircraftCompatible != null ? aircraftCompatible.MappedAircraft : "Current aircraft not supported",
                CurrentLocation = nearest != null ? nearest.Icao : "Not near any known airport",
                CurrentLocationHasRentedAircraft = nearestAirportHasRentedAircraft,
                CurrentAircraftIsCompatible = aircraftCompatible != null
            };
            return playerFlight;
        }

        private CompatibleAircraft IsAircraftCompatible(string model)
        {
            try
            {
                // is aircraft compatible ?
                var compatibleAircraft = _compatibleAircraftCollection.AsQueryable()
                    .FirstOrDefault(x => x.SimAircraft.Contains(model));

                return compatibleAircraft;
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
            catch (ArgumentNullException ex)
            {
                return null;
            }
        }

        private Airport GetNearestAirport(double lat, double lon)
        {
            // get nearest airport
            var latMin = lat -10 / 69.0;
            var latMax = lat + 10 / 69.0;
            var filteredIcaos = _icaoCollection.AsQueryable().Where(x => x.Lat > latMin && x.Lat < latMax).ToList();
            var bestDistance = 99999.0;
            Airport bestIcao = null;
            foreach (var filteredIcao in filteredIcaos)
            {
                var distance = this.distance(lat, lon, filteredIcao.Lat, filteredIcao.Lon, 'K');
                if (distance < bestDistance)
                {
                    bestIcao = filteredIcao;
                    bestDistance = distance;
                }
            }

            return bestIcao;
        }

        private double distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            if ((Math.Abs(lat1 - lat2) < 0.01f) && (Math.Abs(lon1 - lon2) < 0.01f))
            {
                return 0;
            }
            else
            {
                double theta = lon1 - lon2;
                double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
                dist = Math.Acos(dist);
                dist = rad2deg(dist);
                dist = dist * 60 * 1.1515;
                if (unit == 'K')
                {
                    dist = dist * 1.609344;
                }
                else if (unit == 'N')
                {
                    dist = dist * 0.8684;
                }
                return (dist);
            }
        }

        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}
