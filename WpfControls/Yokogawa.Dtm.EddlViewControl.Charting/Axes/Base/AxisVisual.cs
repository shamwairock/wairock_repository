using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public abstract class AxisVisual : DrawingVisual, IAxisRender, IRenderBound
    {
        public static readonly DependencyProperty AxisProperty =
            AxisControl.AxisProperty.AddOwner(typeof(AxisVisual));

        public Axis Axis
        {
            get { return (Axis)GetValue(AxisProperty); }
            set { SetValue(AxisProperty, value); }
        }

        public static readonly DependencyProperty ContentLayoutProperty =
            AxisControl.ContentLayoutProperty.AddOwner(typeof(AxisVisual));

        public AxisContentLayout ContentLayout
        {
            get { return (AxisContentLayout)GetValue(ContentLayoutProperty); }
            set { SetValue(ContentLayoutProperty, value); }
        }

        #region FontFamily
        public static readonly DependencyProperty FontFamilyProperty =
            AxisControl.FontFamilyProperty.AddOwner(typeof(AxisVisual));

        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }
        #endregion

        #region FontSize
        public static readonly DependencyProperty FontSizeProperty =
            AxisControl.FontSizeProperty.AddOwner(typeof(AxisVisual));

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }
        #endregion

        #region FontStyle
        public static readonly DependencyProperty FontStyleProperty =
            AxisControl.FontStyleProperty.AddOwner(typeof(AxisVisual));

        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }
        #endregion

        #region FontWeight
        public static readonly DependencyProperty FontWeightProperty =
            AxisControl.FontWeightProperty.AddOwner(typeof(AxisVisual));

        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }
        #endregion

        #region FontStretch
        public static readonly DependencyProperty FontStretchProperty =
            AxisControl.FontStretchProperty.AddOwner(typeof(AxisVisual));

        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }
        #endregion

        #region TickLabelFontSize
        public static readonly DependencyProperty TickLabelFontSizeProperty =
            AxisControl.TickLabelFontSizeProperty.AddOwner(typeof(AxisVisual));

        public double TickLabelFontSize
        {
            get { return (double)GetValue(TickLabelFontSizeProperty); }
            set { SetValue(TickLabelFontSizeProperty, value); }
        }
        #endregion

        public abstract void Render(AxisVisualContext context);

        public abstract Rect GetRenderBound();
    }
}
