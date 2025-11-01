using System.Collections.Generic;
using System.Linq;
using API.Data.Mappers;
using API.Data.Models;
using API.Domain.Exceptions;
using API.Domain.IRepository;
using API.Domain.Models;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Data
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IMongoCollection<Player> _playerCollection;
        private readonly IMongoCollection<Aircraft> _aircraftCollection;
        private readonly IMongoCollection<Job> _jobCollection;
        private readonly IMapper _mapper;

        public PlayerRepository()
        {
            _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<PlayerRepositoryMappingProfile>(); }).CreateMapper();
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("economy");
            _playerCollection = database.GetCollection<Player>("players");
            _jobCollection = database.GetCollection<Job>("jobs");
            _aircraftCollection = database.GetCollection<Aircraft>("aircrafts");
        }

        public List<PlayerDM> GetAll()
        {
            var players = _playerCollection.Find(new BsonDocument()).ToList();
            return _mapper.Map<List<PlayerDM>>(players);
        }

        public PlayerDM Get(string userName)
        {
            var player = _playerCollection.AsQueryable().SingleOrDefault(x => x.UserName == userName);

            if(player == null)
                throw new RepositoryNotFoundException($"Player {userName} not found");

            var result = _mapper.Map<PlayerDM>(player);
            return result;
        }

        public PlayerDM Update(string userName, PlayerDM player)
        {
            var playerEnt = _playerCollection.AsQueryable().SingleOrDefault(x => x.UserName == userName);

            if (player is null)
                throw new RepositoryNotFoundException($"Player {userName} not found");

            playerEnt.Money = player.Money;

            // jobs
            AddJobs(playerEnt, player);
            RemoveJobs(playerEnt, player);

            // aircraft
            RentedAircraft(playerEnt, player);

            _playerCollection.FindOneAndReplace(x => x.UserName == player.UserName, playerEnt);

            return _mapper.Map<PlayerDM>(playerEnt);
        }

        public PlayerDM Create(string userName)
        {
            var newPlayer = new Player()
            {
                UserName = userName
            };

            _playerCollection.InsertOne(newPlayer);

            return _mapper.Map<PlayerDM>(newPlayer);
        }

        private void RentedAircraft(Player playerEnt, PlayerDM playerDM)
        {

            // unchanged
            if (playerEnt.RentedAircraft != null && playerDM.RentedAircraft != null &&
                playerEnt.RentedAircraft.Id == playerDM.RentedAircraft.Id)
            {
                playerEnt.RentedAircraft.Location = playerDM.RentedAircraft.Location;
                return;
            }
                

            // unrent existing aircraft
            if (playerEnt.RentedAircraft != null && playerDM.RentedAircraft == null)
            {
                var aircraftEnt = _aircraftCollection.AsQueryable()
                    .SingleOrDefault(x => x.Id == playerEnt.RentedAircraft.Id);

                if (aircraftEnt == null)
                    throw new RepositoryNotFoundException($"Aircraft to unassign {playerEnt.RentedAircraft.Id} not found");

                // unassign
                aircraftEnt.IsAssigned = false;
                _aircraftCollection.FindOneAndReplace(x => x.Id == playerEnt.RentedAircraft.Id, aircraftEnt);
                playerEnt.RentedAircraft = null;
            }

            // unrent old aircraft and rent new one
            if (playerEnt.RentedAircraft != null && playerDM.RentedAircraft != null)
            {
                var rentedAircraftEnt = _aircraftCollection.AsQueryable()
                    .SingleOrDefault(x => x.Id == playerEnt.RentedAircraft.Id);

                if (rentedAircraftEnt == null)
                    throw new RepositoryNotFoundException($"Existing rented aircraft {playerEnt.RentedAircraft.Id} not found");

                var newRentedAircraft = _aircraftCollection.AsQueryable()
                    .SingleOrDefault(x => x.Id == playerDM.RentedAircraft.Id);

                if (newRentedAircraft == null)
                    throw new RepositoryNotFoundException($"New aircraft to rent {playerEnt.RentedAircraft.Id} not found");

                if(newRentedAircraft.IsAssigned)
                    throw new RepositoryBadRequestException($"Aircraft {newRentedAircraft.Id} already assigned");

                rentedAircraftEnt.IsAssigned = false;
                _aircraftCollection.FindOneAndReplace(x => x.Id == playerEnt.RentedAircraft.Id, rentedAircraftEnt);

                newRentedAircraft.IsAssigned = true;
                _aircraftCollection.FindOneAndReplace(x => x.Id == playerEnt.RentedAircraft.Id, newRentedAircraft);

                playerEnt.RentedAircraft = newRentedAircraft;
            }

            // no aircraft currently rented, rent new aircraft
            if (playerEnt.RentedAircraft == null && playerDM.RentedAircraft != null)
            {
                var newRentedAircraft = _aircraftCollection.AsQueryable()
                    .SingleOrDefault(x => x.Id == playerDM.RentedAircraft.Id);

                if (newRentedAircraft == null)
                    throw new RepositoryNotFoundException($"New aircraft to rent {playerEnt.RentedAircraft.Id} not found");

                if (newRentedAircraft.IsAssigned)
                    throw new RepositoryBadRequestException($"Aircraft {newRentedAircraft.Id} already assigned");

                newRentedAircraft.IsAssigned = true;

                playerEnt.RentedAircraft = newRentedAircraft;
            }

        }

        private void AddJobs(Player playerEnt, PlayerDM playerDM)
        {
            playerEnt.DepartingJobs.Clear();
            playerEnt.EnrouteJobs.Clear();

            // add jobs
            playerDM.Jobs.ForEach(x =>
            {
                var existingJobEnt = playerEnt.Jobs.SingleOrDefault(j => j.Id == x.Id);
                var jobEnt = _jobCollection.AsQueryable().SingleOrDefault(j => j.Id == x.Id);

                if (jobEnt == null)
                    throw new RepositoryNotFoundException($"Job {x.Id} not found");

                var depJob = playerDM.DepartingJobs.Exists(d => d.Id == jobEnt.Id);
                var enrouteJob = playerDM.EnrouteJobs.Exists(d => d.Id == jobEnt.Id);

                if (existingJobEnt == null)
                {
                    // update job document
                    jobEnt.IsAssigned = true;
                    _jobCollection.FindOneAndReplace(j => j.Id == x.Id, jobEnt);

                    // add job to player
                    playerEnt.Jobs.Add(jobEnt);
                }

                // is a departing job
                if(depJob)
                    playerEnt.DepartingJobs.Add(jobEnt);

                // is an enroute job
                if(enrouteJob)
                    playerEnt.EnrouteJobs.Add(jobEnt);
            });
        }

        private void RemoveJobs(Player playerEnt, PlayerDM playerDM)
        {
            // diff jobs
            var playerDMJobIds = playerDM.Jobs.Select(x => x.Id).ToList();
            var playerEntJobIds = playerEnt.Jobs.Select(x => x.Id).ToList();
            var jobIdsToDelete = playerEntJobIds.Except(playerDMJobIds).ToList();
            var jobsToDelete = playerEnt.Jobs.Where(x => jobIdsToDelete.Contains(x.Id)).ToList();
            
            // unassign jobs
            jobsToDelete.ForEach(x =>
            {
                var jobEnt = _jobCollection.AsQueryable().SingleOrDefault(j => j.Id == x.Id);
                if (jobEnt == null)
                    throw new RepositoryNotFoundException($"Job {x.Id} not found");
                jobEnt.IsAssigned = false;
                _jobCollection.FindOneAndReplace(j => j.Id == x.Id, jobEnt);
            });

            // remove jobs from entity
            playerEnt.Jobs.RemoveAll(x => jobIdsToDelete.Contains(x.Id));
        }

    }
}
