using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public interface IAxisFixRange
    {
        void FixRangeTo(object start, object end);
        void ReleaseFixRange();
    }
}
