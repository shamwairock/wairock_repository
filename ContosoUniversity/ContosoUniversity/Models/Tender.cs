using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public class Tender
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid TenderID { get; set; }

        public decimal Amount { get; set; }

        public TenderType TenderType { get; set; }

        public Guid CashID { get; set; }

        public virtual Cash Cash { get; set; }

        public Guid CardID { get; set; }

        public virtual Card Card { get; set; }
    }
}