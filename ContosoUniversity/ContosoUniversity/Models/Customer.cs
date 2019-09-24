using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }
    }
}