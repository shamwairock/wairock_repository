using System;

namespace BitSpread.Security.DataModel
{
    public class NormalizedOrder
    {
        public double AskVolume { get; set; }
        public DateTime AskTimestamp { get; set; }
        public double BidVolume { get; set; }
        public DateTime BidTimestamp { get; set; }
        public double MatchVolume { get; set; }
        public double Price { get; set; }
        public double ImbalanceVolume { get; set; }
    }
}
