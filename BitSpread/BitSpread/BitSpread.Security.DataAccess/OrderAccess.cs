using System;
using System.Collections.Generic;
using System.Text;
using BitSpread.Security.DataModel;
using BitSpread.Security.Interfaces;

namespace BitSpread.Security.DataAccess
{
    public class OrderAccess : IOrderAccess
    {
        public double GetLastClosingPrice()
        {
            throw new NotImplementedException();
        }

        public List<Order> GetOrders()
        {
            throw new NotImplementedException();
        }
    }
}
