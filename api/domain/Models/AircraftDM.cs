namespace API.Domain.Models
{
    public class AircraftDM
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
    }
}
