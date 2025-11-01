using System.Collections.Generic;
using System.Linq;
using API.Domain.Enums;
using API.Domain.Exceptions;

namespace API.Domain.Models
{
    public class PlayerDM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public float Money { get; set; }
        public PlayerFlightStatus FlightStatus { get; set; } = PlayerFlightStatus.Ground;
        public List<JobDM> EnrouteJobs { get; set; } = new List<JobDM>();
        public List<JobDM> DepartingJobs { get; set; } = new List<JobDM>();
        public List<JobDM> Jobs { get; set; } = new List<JobDM>();
        public AircraftDM RentedAircraft { get; set; }

        public void AddJob(JobDM job)
        {
            var existing = Jobs.SingleOrDefault(x => x.Id == job.Id);

            if (existing != null)
                throw new DomainBadRequestException($"Player {UserName} already has job {job.Id}");

            Jobs.Add(job);

            UpdateDepartingJobs();
        }

        public void RemoveJob(string id)
        {
            var existing = Jobs.SingleOrDefault(x => x.Id == id);

            if (existing == null)
                throw new DomainNotFoundException($"Player {UserName} does not have {id} to remove");

            Jobs.Remove(existing);

            UpdateDepartingJobs();
        }

        public void RentAircraft(AircraftDM aircraft)
        {
            if (RentedAircraft != null)
                throw new DomainBadRequestException($"Player {UserName} already renting aircraft {RentedAircraft.Model} at {RentedAircraft.Location}");

            RentedAircraft = aircraft;

            UpdateDepartingJobs();
        }

        public void ReleaseRentedAircraft()
        {
            if (RentedAircraft == null)
                throw new DomainBadRequestException($"Player {UserName} not renting any aircraft");

            RentedAircraft = null;

            UpdateDepartingJobs();
        }

        public void StartFlight()
        {
            EnrouteJobs.Clear();
            EnrouteJobs.AddRange(DepartingJobs);
            DepartingJobs.Clear();
            FlightStatus = PlayerFlightStatus.Enroute;
        }

        public void EndFlight(string currentIcao)
        {
            EnrouteJobs.ForEach(x =>
            {
                if (x.ToIcao == currentIcao)
                {
                    Money += x.Pay;
                    RemoveJob(x.Id);
                }
            });
            EnrouteJobs.Clear();
            RentedAircraft.Location = currentIcao;
            UpdateDepartingJobs();
            FlightStatus = PlayerFlightStatus.Ground;
        }

        private void UpdateEnrouteJobs()
        {
            if (RentedAircraft == null)
                return;

            EnrouteJobs.Clear();

            long takenSeats = 0;
            long takenWeight = 0;
            var possibleJobs = Jobs.Where(x => x.FromIcao == RentedAircraft.Location).ToList();

            possibleJobs.ForEach(x =>
            {
                if (x.Type == "Passanger" && takenSeats + x.Amount <= RentedAircraft.Seats)
                {
                    EnrouteJobs.Add(x);
                    takenSeats += x.Amount;
                }

                if (x.Type == "Cargo" && takenWeight + x.Amount <= RentedAircraft.MTOW)
                {
                    EnrouteJobs.Add(x);
                    takenWeight += x.Amount;
                }
            });
        }

        private void UpdateDepartingJobs()
        {
            DepartingJobs.Clear();

            if (RentedAircraft == null)
                return;


            long takenSeats = 0;
            long takenWeight = 0;
            var possibleJobs = Jobs.Where(x => x.FromIcao == RentedAircraft.Location).ToList();

            possibleJobs.ForEach(x =>
            {
                if (x.Type == "Passanger" && takenSeats + x.Amount <= RentedAircraft.Seats)
                {
                    DepartingJobs.Add(x);
                    takenSeats += x.Amount;
                }

                if (x.Type == "Cargo" && takenWeight + x.Amount <= RentedAircraft.MTOW)
                {
                    DepartingJobs.Add(x);
                    takenWeight += x.Amount;
                }
            });
        }
    }
}
