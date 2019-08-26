using System;
using System.Collections.Generic;
using System.Text;

namespace BitSpread.Security.DataModel
{
    public class NormalizedOrder
    {
        public double AskVolume { get; set; }
        public double BidVolume { get; set; }

        public double MatchVolume { get; set; }
        public double Price { get; set; }

        public double ImbalanceVolume { get; set; }
    }
}
