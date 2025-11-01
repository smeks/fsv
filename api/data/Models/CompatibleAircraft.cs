using System.Collections.Generic;

namespace API.Data.Models
{
    public class CompatibleAircraft
    {
        public string Id { get; set; }
        public List<string> SimAircraft { get; set; }
        public string MappedAircraft { get; set; }
    }
}
