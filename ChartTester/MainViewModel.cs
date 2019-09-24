using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Baml2006;
using ChartTester.Annotations;
using Yokogawa.Dtm.EddlViewControl.Charting;

namespace ChartTester
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<DataSeries> dataSeriesSources;

        public ObservableCollection<DataSeries> DataSeriesSources
        {
            get { return dataSeriesSources;}
            set { dataSeriesSources = value; }
        }

        public MainViewModel()
        {
            if (DataSeriesSources == null)
            {
                DataSeriesSources = new ObservableCollection<DataSeries>();
            }

            var dataSeries = new DataSeries();
            IAxisDataType yAxisDataType = new DoubleAxisData();
            IAxisDataType xAxisDataType = new DoubleAxisData();

            dataSeries.XAxis = new LinearXAxis(xAxisDataType);
            dataSeries.YAxis = new LinearYAxis(yAxisDataType);

            dataSeries.VisualType = ChartVisualType.VERTICALBAR;
            dataSeries.SeriesName = "My Test Series";
            IDataPoints dataPoints  = new DataPoints<double, double>();
            dataPoints.Initialize(dataSeries);
           
            var point1 = new DataPoint<double, double>(1, 1, false);
            var point2 = new DataPoint<double, double>(2, 2, false);
            var point3 = new DataPoint<double, double>(3, 3, false);

            dataPoints.Add(point1);
            dataPoints.Add(point2);
            dataPoints.Add(point3);
            dataPoints.Add(point1);
            dataPoints.Add(point3);
            dataPoints.Add(point3);
            dataPoints.Add(point3);
            dataPoints.Add(point1);
            dataPoints.Add(point3);
            dataPoints.Add(point1);
            dataPoints.Add(point3);
            dataPoints.Add(point1);
            dataPoints.Add(point3);
            dataSeries.Points = dataPoints;

            dataSeries.PlotFlowAxisOption  = PlotFlowAxisOptions.XAxis;

            dataSeries.BarWidth = 20;
            dataSeries.IsVisible = true;


            dataSeriesSources.Add(dataSeries);
        }
    }
}
