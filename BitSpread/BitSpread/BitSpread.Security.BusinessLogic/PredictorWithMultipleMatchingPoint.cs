using System;
using System.Collections.Generic;
using System.Linq;
using BitSpread.Security.DataAccess;
using BitSpread.Security.DataModel;
using BitSpread.Security.Interfaces;

namespace BitSpread.Security.BusinessLogic
{
    public class PredictorWithMultipleMatchingPoint : IPredictor
    {
        public Tuple<double, double> PredictOrder(IList<NormalizedOrder> orders, double lastClosingPrice)
        {
            var maxMatchingVolume = orders.Max(x => x.MatchVolume);

            var highestMatchingOrders = orders.Where(x => Math.Abs(x.MatchVolume - maxMatchingVolume) < CalConst.TOLERANCE).ToList();

            var minimumImbalanceVolume = highestMatchingOrders.Min(x => x.ImbalanceVolume);

            var matchingOrdersWithMinimumImbalance =
                highestMatchingOrders.Where(x => Math.Abs(x.ImbalanceVolume - minimumImbalanceVolume) < CalConst.TOLERANCE).ToList();

            if (matchingOrdersWithMinimumImbalance.Count == 1)
            {
                var matchingOrderWithMinimumImbalance = matchingOrdersWithMinimumImbalance.First();
                var equilibriumPrice = matchingOrderWithMinimumImbalance.Price;

                var sellingOrders = orders.Where(x => x.Price <= equilibriumPrice);
                var sellingVolume = sellingOrders.Sum(x => x.AskVolume);

                return new Tuple<double, double>(equilibriumPrice, sellingVolume);
            }
            else
            {
                IList<Tuple<NormalizedOrder, double>> differencesList = new List<Tuple<NormalizedOrder, double>>();

                foreach (var matchingOrderWithMinimumImbalance in matchingOrdersWithMinimumImbalance)
                {
                    differencesList.Add(new Tuple<NormalizedOrder, double>(matchingOrderWithMinimumImbalance, Math.Abs(matchingOrderWithMinimumImbalance.Price - lastClosingPrice)));
                }

                var sortedDifferencesList = differencesList.OrderBy(x => x.Item2).Select(x => x.Item1).ToList();

                var minimumPriceDifferenceOrder = sortedDifferencesList.First();
                var equilibriumPrice = minimumPriceDifferenceOrder.Price;

                var sellingOrders = orders.Where(x => x.Price <= equilibriumPrice);
                var sellingVolume = sellingOrders.Sum(x => x.AskVolume);

                return new Tuple<double, double>(equilibriumPrice, sellingVolume);
            }
        }
    }
}
