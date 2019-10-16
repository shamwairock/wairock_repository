using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid OrderID { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus Status { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal ServiceCharges { get; set; }

        public decimal Tax { get; set; }

        public Guid CustomerID { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }

        public virtual ICollection<Tender> Tenders { get; set; }
    }
}