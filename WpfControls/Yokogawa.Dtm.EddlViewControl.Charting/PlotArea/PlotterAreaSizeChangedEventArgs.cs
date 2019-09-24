using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class PlotterAreaSizeChangedEventArgs : EventArgs
    {
        public bool WidthChanged { get; set; }
        public bool HeightChanged { get; set; }
        public Size Size { get; set; }
    }

    public delegate void PlotterAreaSizeChangedEvent(object sender, PlotterAreaSizeChangedEventArgs e);
}
