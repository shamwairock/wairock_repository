using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    internal static class DebugTraceLog
    {
        public static void WriteLine(string msg, bool allwaysShow = false)
        {
            if (allwaysShow)
            {
                System.Diagnostics.Debug.WriteLine(msg);
            }
            else
            {
#if SHOW_DEBUG_LOG
            System.Diagnostics.Debug.WriteLine(msg);
#else

#endif
            }
        }
    }
}
