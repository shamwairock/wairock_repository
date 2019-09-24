using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }

        public int Quantity { get; set; }

        public decimal SubTotal { get; set; }

        public Guid ItemId { get; set; }

        public virtual Item Item { get; set; }
    }
}