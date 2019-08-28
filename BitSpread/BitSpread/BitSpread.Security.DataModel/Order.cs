using System;

namespace BitSpread.Security.DataModel
{
    public class Order
    {
        public string OrderId { get; set; }
        public DateTime Timestamp { get; set; }
        public double Volume { get; set; }
        public double Price { get; set; }
        public OrderType OrderType { get; set; }
        public string SecurityCode { get; set; }
    }
}
