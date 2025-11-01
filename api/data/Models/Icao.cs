using System;

namespace API.Data.Models
{
    public class Airport
    {
        public string Id { get; set; }
        public string Icao { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Type { get; set; }
        public Int32 Size { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
