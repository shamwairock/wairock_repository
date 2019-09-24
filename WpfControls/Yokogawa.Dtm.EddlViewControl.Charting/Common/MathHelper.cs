using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    internal class MathHelper
    {
        internal static int Clamp(int value, int min, int max)
        {
            return Math.Max(min, Math.Min(value, max));
        }
    }
}
