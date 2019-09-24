using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public class Cart
    {
        public Guid CartId { get; set; }
        public DateTime CreateTimestamp { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}