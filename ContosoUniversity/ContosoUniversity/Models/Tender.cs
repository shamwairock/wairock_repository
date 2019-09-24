using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public class Tender
    {
        public Guid TenderId { get; set; }
        public decimal Amount { get; set; }
        public Guid PaymentId { get; set; }
        public virtual Payment Payment { get; set; }
    }
}