using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class ChartBar : DataSeriesView
    {
        public ChartBar()
        {
            ChartBarVisual itemVisual = new ChartBarVisual();

            BindingOperations.SetBinding(itemVisual, DataSeriesProperty, new Binding("DataSeries") { Source = this });

            visuals.Add(itemVisual);
        }
    }
}
