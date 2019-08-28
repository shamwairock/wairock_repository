using System;
using System.Collections.Generic;
using BitSpread.Security.DataModel;

namespace BitSpread.Security.BusinessLogic
{
    public class PredictorWithNoMatchingPoint : IPredictor
    {
        public Tuple<double, double> PredictOrder(IList<NormalizedOrder> orders, double lastClosingPrice)
        {
            return new Tuple<double, double>(0,0);
        }
    }
}
