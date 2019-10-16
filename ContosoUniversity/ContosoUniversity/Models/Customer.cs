using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid CustomerID { get; set; }

        public Guid AddressID { get; set; }

        public virtual Address Address { get; set; }

        public virtual ICollection<Card> CreditCards { get; set; }
    }
}