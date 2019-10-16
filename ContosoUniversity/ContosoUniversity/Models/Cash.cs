using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public class Cash
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid CashID { get; set; }

        public decimal ReceivedAmount { get; set; }

        public decimal ChangedAmount { get; set; }

        public decimal RoundingAmount { get; set; }
    }
}