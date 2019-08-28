using System;
using System.Collections.Generic;
using System.Linq;
using BitSpread.Security.DataModel;
using BitSpread.Security.Exceptions;

namespace BitSpread.Security.BusinessLogic
{
    public static class PredictorFactory
    {
        public static IPredictor CreateInstance(IList<NormalizedOrder> orders)
        {
            if (orders == null)
            {
                throw new ArgumentNullException(nameof(orders));
            }

            try
            {
                var maxMatchingVolume = orders.Max(x => x.MatchVolume);

                var highestVolumeOrders =
                    orders.Where(x => Math.Abs(x.MatchVolume - maxMatchingVolume) < CalConst.TOLERANCE).ToList();

                var matchingPoint = highestVolumeOrders.Count;

                switch (matchingPoint)
                {
                    case 0:
                        return new PredictorWithNoMatchingPoint();
                    case 1:
                        return new PredictorWithOneMatchingPoint();
                    default:
                        return new PredictorWithMultipleMatchingPoint();
                }
            }
            catch (Exception ex)
            {
                throw new PredictorNotFoundException("Failed to create predictor.", ex);
            }
        }
    }
}
