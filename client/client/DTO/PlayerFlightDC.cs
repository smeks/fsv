using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Client.DTO
{
    public class PlayerFlightDC
    {
        public PlayerDC Player { get; set; }
        public string CurrentAircraft { get; set; }
        public string CurrentLocation { get; set; }
        public bool CurrentAircraftIsCompatible { get; set; }
        public bool CurrentLocationHasRentedAircraft { get; set; }
    }
}
