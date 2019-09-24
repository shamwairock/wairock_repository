using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class ChartScatteredPoints : DataSeriesView
    {
        public ChartScatteredPoints()
        {
            ChartScatteredPointsVisual itemVisual = new ChartScatteredPointsVisual();

            BindingOperations.SetBinding(itemVisual, DataSeriesProperty, new Binding("DataSeries") { Source = this });
           
            visuals.Add(itemVisual);
        }
    }
}
