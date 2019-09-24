using BitSpread.Security.DataAccess;
using BitSpread.Security.Interfaces;
using System;

namespace BitSpread.Security.DataAccessFactory
{
    public static class DataAccessFactory
    {
        private static IOrderAccessService orderAccessService;

        public static IOrderAccessService CreateInstance()
        {
            if(orderAccessService == null)
            {
                orderAccessService = new OrderAccessService(true, @"MockOrders.txt", @"MockOrdersSummary.txt");
            }

            return orderAccessService;
        }
    }
}
