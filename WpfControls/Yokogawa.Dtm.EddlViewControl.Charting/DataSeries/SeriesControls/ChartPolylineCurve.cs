using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class ChartPolylineCurve : DataSeriesView
    {
        private static int flagcount = 0;
        public ChartPolylineCurve()
        {
            System.Diagnostics.Debug.WriteLine(string.Format("new ChartPolylineCurve() --> {0}", flagcount++));

            ChartPolylineCurveVisual itemVisual = new ChartPolylineCurveVisual();
            BindingOperations.SetBinding(itemVisual, DataSeriesProperty, new Binding("DataSeries") { Source = this });
            visuals.Add(itemVisual);
        }
    }
}
