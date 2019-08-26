using System;
using System.Collections.Generic;
using System.Linq;
using BitSpread.Security.DataModel;

namespace BitSpread.Security.BusinessLogic
{
    public class Calculator : ICalculator
    {
        private const double TOLERANCE = 0.001;

        public double GetMatchVolume(double bidVolume, double askVolume)
        {
            return bidVolume >= askVolume ? askVolume : bidVolume;
        }

        public double GetAbsImbalanceVolume(double bidVolume, double askVolume)
        {
            return Math.Abs(bidVolume - askVolume);
        }

        public List<NormalizedOrder> GetNormalizedOrders(List<Order> orders)
        {
            var bidOrders = orders.Where(x => x.OrderType == OrderType.BUY).OrderByDescending(x => x.Price).ToList();
            var askOrders = orders.Where(x => x.OrderType == OrderType.SELL).OrderBy(x => x.Price).ToList();

            var leftOuterJoinOrders = from bidOrder in bidOrders
                join askOrder in askOrders
                    on bidOrder.Price equals askOrder.Price
                    into temp
                from askOrder in temp.DefaultIfEmpty()
                select new {bidOrder, askOrder};

            var rightOuterJoinOrders = from askOrder in askOrders
                join bidOrder in bidOrders
                    on askOrder.Price equals bidOrder.Price
                    into temp
                from bidOrder in temp.DefaultIfEmpty()
                select new {bidOrder, askOrder};

            var fullJoinOrders = leftOuterJoinOrders.Union(rightOuterJoinOrders).ToList();

            var normalizedOrders = new List<NormalizedOrder>();

            foreach (var jointOrder in fullJoinOrders)
            {
                var bidOrder = jointOrder.bidOrder;
                var askOrder = jointOrder.askOrder;

                double bidVolume = 0;
                double askVolume = 0;
                double price = 0;
                if (bidOrder != null)
                {
                    bidVolume = bidOrder.Volume;
                    price = bidOrder.Price;
                }

                if (askOrder != null)
                {
                    askVolume = askOrder.Volume;
                    price = askOrder.Price;
                }

                normalizedOrders.Add(new NormalizedOrder()
                {
                    MatchVolume = GetMatchVolume(bidVolume, askVolume),
                    ImbalanceVolume = GetAbsImbalanceVolume(bidVolume, askVolume),
                    Price = price,
                    AskVolume = askVolume,
                    BidVolume = bidVolume
                });
            }

            return normalizedOrders.OrderBy(x => x.Price).ToList();
        }
    }
}
