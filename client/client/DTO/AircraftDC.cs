using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Client.DTO
{
    public class AircraftDC
    {
        public string Id { get; set; }
        public string Model { get; set; }
        public long Crew { get; set; }
        public long Seats { get; set; }
        public long Cruise { get; set; }
        public long FuelCapacity { get; set; }
        public long Fuel { get; set; }
        public long GPH { get; set; }
        public string FuelType { get; set; }
        public long MTOW { get; set; }
        public long EmptyWeight { get; set; }
        public float RentCostPerHour { get; set; }
        public string Location { get; set; }
        public bool IsAssigned { get; set; }
    }
}
