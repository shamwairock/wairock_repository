using System;
using System.Collections.Generic;
using System.Linq;
using BitSpread.Security.DataModel;

namespace BitSpread.Security.BusinessLogic
{
    public class PredictorWithOneMatchingPoint : IPredictor
    {
        public Tuple<double, double> PredictOrder(IList<NormalizedOrder> orders, double lastClosingPrice)
        {
            var highestMatchingOrder = orders.OrderByDescending(x => x.MatchVolume).FirstOrDefault();

            if (highestMatchingOrder != null)
            {
                var equilibriumPrice = highestMatchingOrder.Price;

                var sellingOrders = orders.Where(x => x.Price <= equilibriumPrice);

                var sellingVolume = sellingOrders.Sum(x => x.AskVolume);

                return new Tuple<double, double>(equilibriumPrice, sellingVolume);
            }
            else
            {
                return new Tuple<double, double>(0, 0);
            }
        }
    }
}
