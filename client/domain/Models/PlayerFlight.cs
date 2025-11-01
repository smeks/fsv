namespace Economy.Domain.Models
{
    public class PlayerFlight
    {
        public Player Player { get; set; }
        public string CurrentAircraft { get; set; }
        public string CurrentLocation { get; set; }
        public bool CurrentAircraftIsCompatible { get; set; }
        public bool CurrentLocationHasRentedAircraft { get; set; }
    }
}
