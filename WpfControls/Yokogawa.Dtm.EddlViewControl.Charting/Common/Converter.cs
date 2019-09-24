using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    //[ValueConversion(typeof(Scale), typeof(TickLabelDrawing))]
    //public class ScaleLabelDrawingConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        LinearScale linearScale = value as LinearScale;
    //        if (linearScale != null)
    //        {
    //            return new LinearTickLabelDrawing();
    //        }
            
    //        LogarithmicScale logarithmicScale = value as LogarithmicScale;
    //        if (logarithmicScale != null)
    //        {
    //            return new LogarithmicTickLabelDrawing();
    //        }
    //        return null;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    [ValueConversion(typeof(AxisZoneTypes), typeof(Visibility))]
    public class YAxisZoneVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is AxisZoneTypes && ((AxisZoneTypes)value) == AxisZoneTypes.AUTOZONE)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(AxisZoneTypes), typeof(Visibility))]
    public class YAxisZoneHiddenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is AxisZoneTypes && ((AxisZoneTypes)value) == AxisZoneTypes.AUTOZONE)
            {
                return Visibility.Hidden;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VisibletoVisibilityCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || (!( value is bool)))
                return DependencyProperty.UnsetValue;
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Visibility.Visible.Equals(value) ? true : false;
        }
    }

    public class MarginTopConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || (!(value is Thickness)))
                return DependencyProperty.UnsetValue;
            return ((Thickness)value).Top;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class MarginLeftConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || (!(value is Thickness)))
                return DependencyProperty.UnsetValue;
            return ((Thickness)value).Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class MarginBottomConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || (!(value is Thickness)))
                return DependencyProperty.UnsetValue;
            return ((Thickness)value).Bottom;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class MarginRightConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || (!(value is Thickness)))
                return DependencyProperty.UnsetValue;
            return ((Thickness)value).Right;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class YAxisTitlePosTransConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || (!(values.Count() >= 4)))
                return DependencyProperty.UnsetValue;

            var pos = (AxisPosition)values[0];
            var topLeftPosPixels = (double)values[1];
            var bottomRightPosPixels = (double)values[2];
            var extent = (double)values[3];
            double infoPosTrans = (topLeftPosPixels + bottomRightPosPixels - extent) / 2;
            switch (pos)
            {
                case AxisPosition.AtTop:
                    infoPosTrans = topLeftPosPixels;
                    break;
                default:
                    break;
            }
            return infoPosTrans;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class YAxisPosTransConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || (!(value is AxisPosition)))
                return DependencyProperty.UnsetValue;

            var pos = (AxisPosition)value;
            var alignment = System.Windows.HorizontalAlignment.Right;
            switch (pos)
            {
                case AxisPosition.AtLeft:
                    alignment = HorizontalAlignment.Left;
                    break;
                case AxisPosition.AtCenter:
                    alignment = HorizontalAlignment.Center;
                    break;
                default:
                    break;
            }

            return alignment;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
