namespace Economy.Domain.Models
{
    public class FlightData
    {
        public int Altitude { get; set; }
        public int Altitude_agl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Onground { get; set; }
        public int WheelRpm { get; set; }
        public int ParkingBreak { get; set; }
        public int GroundVelocity { get; set; }
        public int Cht1 { get; set; }
        public int Cht2 { get; set; }
        public int Cht3 { get; set; }
        public int Cht4 { get; set; }
        public int Mixture1 { get; set; }
        public int Mixture2 { get; set; }
        public int Mixture3 { get; set; }
        public int Mixture4 { get; set; }
        public int Rpm1 { get; set; }
        public int Rpm2 { get; set; }
        public int Rpm3 { get; set; }
        public int Rpm4 { get; set; }
        public int Combustion1 { get; set; }
        public int Combustion2 { get; set; }
        public int Combustion3 { get; set; }
        public int Combustion4 { get; set; }
        public float TotalFuel { get; set; }
        public int ZuluTime { get; set; }
        public float RpmPercentage { get; set; }
        public int TimeOfDay { get; set; }
        public int Visibility { get; set; }
        public float Crosswind { get; set; }
    }
}
