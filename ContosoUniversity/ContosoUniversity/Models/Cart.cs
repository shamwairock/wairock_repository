using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public class Cart
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid CartID { get; set; }

        public DateTime CreateTimestamp { get; set; }

        public Guid OrderID { get; set; }

        public Order Order { get; set; }
      
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}