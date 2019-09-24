using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class ChartVerticalBar : DataSeriesView
    {
        public ChartVerticalBar()
        {
            ChartVerticalBarVisual itemVisual = new ChartVerticalBarVisual();

            BindingOperations.SetBinding(itemVisual, DataSeriesProperty, new Binding("DataSeries") { Source = this });

            visuals.Add(itemVisual);
        }
    }
}
