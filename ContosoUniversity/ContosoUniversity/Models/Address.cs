using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public class Address
    {
        public Guid AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string PostCode { get; set; }
    }
}