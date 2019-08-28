using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BitSpread.Security.DataModel;
using BitSpread.Security.Exceptions;
using BitSpread.Security.Interfaces;

namespace BitSpread.Security.DataAccess
{
    public class OrderAccessService : IOrderAccessService
    {
        private readonly IOrderAccess orderAccess;

        public OrderAccessService()
        {
            orderAccess = new OrderAccess();
        }

        public OrderAccessService(bool isMock, string mockOrdersPath, string mockOrdersSummaryPath)
        {
            if (isMock)
            {
                orderAccess = new OrderAccessMock();
                ((OrderAccessMock) orderAccess).MockOrdersSummaryPath = mockOrdersSummaryPath;
                ((OrderAccessMock) orderAccess).MockOrdersPath = mockOrdersPath;
            }
            else
            {
                orderAccess = new OrderAccess();
            }
        }

        public List<Order> GetOrders()
        {
            return orderAccess.GetOrders();
        }

        public double GetLastClosingPrice()
        {
            return orderAccess.GetLastClosingPrice();
        }
    }
}
