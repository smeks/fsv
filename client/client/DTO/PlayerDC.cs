using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Economy.Domain.Enum;

namespace Economy.Client.DTO
{
    public class PlayerDC
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public float Money { get; set; }
        public PlayerFlightStatus FlightStatus { get; set; }
        public List<JobDC> DepartingJobs { get; set; }
        public List<JobDC> Jobs { get; set; }
        public AircraftDC RentedAircraft { get; set; }
    }
}
