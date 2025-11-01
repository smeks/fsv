using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Models
{
    public class Icao
    {
        public string ICAO { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
