using System;
using System.Collections.Generic;
using System.Linq;
using BitSpread.Security.DataModel;

namespace BitSpread.Security.BusinessLogic
{
    public class Calculator : ICalculator
    {
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
                var bidTimestamp = DateTime.MinValue;
                var askTimestamp = DateTime.MinValue;

                if (bidOrder != null)
                {
                    bidVolume = bidOrder.Volume;
                    bidTimestamp = bidOrder.Timestamp;
                    price = bidOrder.Price;
                }

                if (askOrder != null)
                {
                    askVolume = askOrder.Volume;
                    askTimestamp = askOrder.Timestamp;
                    price = askOrder.Price;
                }

                normalizedOrders.Add(new NormalizedOrder()
                {
                    MatchVolume = GetMatchVolume(bidVolume, askVolume),
                    ImbalanceVolume = GetAbsImbalanceVolume(bidVolume, askVolume),
                    Price = price,
                    AskVolume = askVolume,
                    AskTimestamp =  askTimestamp,
                    BidVolume = bidVolume,
                    BidTimestamp = bidTimestamp
                });
            }

            return normalizedOrders.OrderBy(x => x.Price).ToList();
        }

        public Tuple<double, double> PredictOpeningPrice(List<NormalizedOrder> orders, double lastClosingPrice)
        {
            var predictor = PredictorFactory.CreateInstance(orders);

            return predictor.PredictOrder(orders, lastClosingPrice);

        }
    }
}
