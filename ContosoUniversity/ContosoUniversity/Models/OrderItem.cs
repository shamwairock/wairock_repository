using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public class OrderItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid OrderItemID { get; set; }

        public int Quantity { get; set; }

        public decimal SubTotal { get; set; }

        public Guid CartID { get; set; }

        public Guid ItemID { get; set; }

        public virtual Item Item { get; set; }
    }
}