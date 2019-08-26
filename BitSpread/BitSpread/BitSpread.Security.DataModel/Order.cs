using System;
using System.Collections.Generic;
using System.Text;

namespace BitSpread.Security.DataModel
{
    public class Order
    {
        public DateTime Timestamp { get; set; }
        public double Volume { get; set; }
    
        public double Price { get; set; }
        public OrderType OrderType { get; set; }
    }
}
