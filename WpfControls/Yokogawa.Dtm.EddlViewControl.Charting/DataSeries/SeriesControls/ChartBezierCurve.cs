using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class ChartBezierCurve : DataSeriesView
    {
        public ChartBezierCurve()
        {
            ChartBezierCurveVisual itemVisual = new ChartBezierCurveVisual();

            BindingOperations.SetBinding(itemVisual, DataSeriesProperty, new Binding("DataSeries") { Source = this });
           
            visuals.Add(itemVisual);
        }
    }
}
