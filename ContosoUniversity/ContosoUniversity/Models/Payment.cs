using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public abstract class Payment
    {
        public Guid PaymentId { get; set; }
        public TenderType TenderType { get; set; }
    }
}