using System;
using System.Collections.Generic;
using System.Text;
using BitSpread.Security.DataModel;

namespace BitSpread.Security.Interfaces
{
    public interface IOrderAccess
    {
        List<Order> GetOrders();
        double GetLastClosingPrice();
    }
}
