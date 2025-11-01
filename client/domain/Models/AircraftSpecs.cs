namespace Economy.Domain.Models
{
    public class AircraftSpecs
    {
        public string Title { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int NumEngines { get; set; }
        public int EngineType { get; set; }
        public float FuelCenter { get; set; }
        public float FuelCenter2 { get; set; }
        public float FuelCenter3 { get; set; }
        public float FuelLeftMain { get; set; }
        public float FuelLeftAux { get; set; }
        public float FuelLeftTip { get; set; }
        public float FuelRightMain { get; set; }
        public float FuelRightAux { get; set; }
        public float FuelRightTip { get; set; }
        public float FuelExt1 { get; set; }
        public float FuelExt2 { get; set; }
        public int EmptyWeight { get; set; }
        public int MaxGrossWeight { get; set; }
        public int EstimateCruise { get; set; }
        public float EstimateFuelFlow { get; set; }
        public int NumPayloadStations { get; set; }
        public bool IsPlaneCompatible { get; set; }
        public bool IsRented { get; set; }
    }
}
