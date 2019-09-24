using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoBookstore.Models
{
    public enum OrderStatus
    {
        Pending,
        AwaitingPayment,
        AwaitingShipment,
        AwaitingPickup,
        Completed,
        Shipped,
        Cancelled,
        Refunded
    }
}