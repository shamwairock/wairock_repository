using System;
using System.Collections.Generic;
using BitSpread.Security.DataModel;

namespace BitSpread.Security.BusinessLogic
{
    public interface IPredictor
    {
        /// <summary>
        /// Tuple 1 = Price
        /// Tuple 2 = Volume
        /// </summary>
        Tuple<double, double> PredictOrder(IList<NormalizedOrder> orders, double lastClosingPrice);
    }
}
