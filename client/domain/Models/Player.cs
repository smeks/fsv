using System.Collections.Generic;
using Economy.Domain.Enum;

namespace Economy.Domain.Models
{
    public class Player
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public float Money { get; set; }
        public PlayerFlightStatus FlightStatus { get; set; }
        public List<Job> DepartingJobs { get; set; }
        public List<Job> Jobs { get; set; }
        public Aircraft RentedAircraft { get; set; }
    }
}
