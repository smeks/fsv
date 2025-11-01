namespace API.Domain.Models
{
    public class PlayerFlightDM
    {
        public PlayerDM Player { get; set; }
        public string CurrentAircraft { get; set; }
        public string CurrentLocation { get; set; }
        public bool CurrentAircraftIsCompatible { get; set; }
        public bool CurrentLocationHasRentedAircraft { get; set; }
    }
}
