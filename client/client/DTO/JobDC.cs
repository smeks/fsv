using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Client.DTO
{
    public class JobDC
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string FromIcao { get; set; }
        public string ToIcao { get; set; }
        public long Amount { get; set; }
        public string AmountUom { get; set; }
        public string Type { get; set; }
        public long Weight { get; set; }
        public string WeightUom { get; set; }
        public float Pay { get; set; }
        public bool IsAssigned { get; set; }
    }
}
