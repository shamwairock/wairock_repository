using System;
using System.Diagnostics;
using BitSpread.Security.DataAccess;
using BitSpread.Security.Interfaces;
using Xunit;

namespace BitSpread.Security.BusinessLogic.XUnitTest
{
    public class CalculatorUnitTest
    {
        [Fact]
        public void TestGetMatchVolume()
        {
            var calculator = new Calculator();
            var matchVolume1 = calculator.GetMatchVolume(25, 50);
            Assert.Equal(25, matchVolume1);
            var matchVolume2 = calculator.GetMatchVolume(50, 25);
            Assert.Equal(25, matchVolume2);
            var matchVolume3 = calculator.GetMatchVolume(0, 50);
            Assert.Equal(0, matchVolume3);
            var matchVolume4 = calculator.GetMatchVolume(50, 0);
            Assert.Equal(0, matchVolume4);
        }

        [Fact]
        public void TestGetNormalizedOrders()
        {
            IOrderAccessService orderAccessService = new OrderAccessService();
            var orders = orderAccessService.GetOrders();

            var calculator = new Calculator();
            var normalizedOrders = calculator.GetNormalizedOrders(orders);

            normalizedOrders.ForEach(order =>
            {
                Trace.WriteLine(
                    $"{order.BidVolume}|{order.Price}|{order.MatchVolume}|{order.AskVolume}");
            });

            Assert.Equal(9, normalizedOrders.Count);
        }
    }
}