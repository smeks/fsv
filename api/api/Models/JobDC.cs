namespace API.Models
{
    public class JobDC
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string FromIcao { get; set; }
        public string ToIcao { get; set; }
        public long Amount { get; set; }
        public string AmountUom { get; set; }
        public string Type { get; set; }
        public long Weight { get; set; }
        public string WeightUom { get; set; }
    }
}
