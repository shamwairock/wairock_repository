using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class ChartHorizontalBar : DataSeriesView
    {
        public ChartHorizontalBar()
        {
            ChartHorizontalBarVisual itemVisual = new ChartHorizontalBarVisual();

            BindingOperations.SetBinding(itemVisual, DataSeriesProperty, new Binding("DataSeries") { Source = this });

            visuals.Add(itemVisual);
        }
    }
}
