using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Models
{
    public class CurrentLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ICAO { get; set; }
    }
}
