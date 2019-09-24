using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    internal class RoundHelper
    {
        internal static double Round(double number, int rem)
        {
            if (rem <= 0)
            {
                rem = MathHelper.Clamp(-rem, 0, 15);
                return Math.Round(number, rem);
            }
            else
            {
                double pow = Math.Pow(10, rem - 1);
                double val = pow * Math.Round(number / Math.Pow(10, rem - 1));
                return val;
            }
        }
    }  
}
