using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionDataFactory
{
    public class Salvage
    {
        public long SalvageId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MinimumValue { get; set; }
        public double IncreaseRatio { get; set; }
        public DateTime BidDate { get; set; }
        public int Status { get; set; }
    }
}
