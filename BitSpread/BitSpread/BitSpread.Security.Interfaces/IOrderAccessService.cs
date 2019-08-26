using System;
using System.Collections.Generic;
using BitSpread.Security.DataModel;

namespace BitSpread.Security.Interfaces
{
    public interface IOrderAccessService
    {
        List<Order> GetOrders();
    }
}
