﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public class Card
    {
        public Guid CardId { get; set; }

        public string CardName { get; set; }

        public string CardNo { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public int CVV { get; set; }

        public CardType CardType { get; set; }

        public Guid CustomerID { get; set; }

        public virtual Customer Customer { get; set; }
    }
}