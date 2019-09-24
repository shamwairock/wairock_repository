using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public abstract class AxisControl : FrameworkElement, IUpdate
    {
        protected VisualCollection visuals;

        public AxisControl()
        {
            this.SnapsToDevicePixels = true;
            visuals = new VisualCollection(this);
        }

        protected override int VisualChildrenCount
        {
            get { return visuals.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index >= visuals.Count)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            return visuals[index];
        }

        public static readonly DependencyProperty AxisProperty =
            DependencyProperty.Register("Axis", typeof(Axis), typeof(AxisControl), 
                new PropertyMetadata(null));

        public Axis Axis
        {
            get { return (Axis)GetValue(AxisProperty); }
            set { SetValue(AxisProperty, value); }
        }

        public static readonly DependencyProperty ContentLayoutProperty =
            DependencyProperty.Register("ContentLayout", typeof(AxisContentLayout), typeof(AxisControl),
                new PropertyMetadata(AxisContentLayout.AtBelow));

        public AxisContentLayout ContentLayout
        {
            get { return (AxisContentLayout)GetValue(ContentLayoutProperty); }
            set { SetValue(ContentLayoutProperty, value); }
        }

        #region FontFamily
        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(AxisControl),
                new PropertyMetadata(SystemFonts.MessageFontFamily));

        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }
        #endregion

        #region FontSize
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(AxisControl),
                new PropertyMetadata(11.0));

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }
        #endregion

        #region FontStyle
        public static readonly DependencyProperty FontStyleProperty =
            DependencyProperty.Register("FontStyle", typeof(FontStyle), typeof(AxisControl),
                new PropertyMetadata(FontStyles.Normal));

        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }
        #endregion

        #region FontWeight
        public static readonly DependencyProperty FontWeightProperty =
            DependencyProperty.Register("FontWeight", typeof(FontWeight), typeof(AxisControl),
                new PropertyMetadata(FontWeights.Normal));

        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }
        #endregion

        #region FontStretch
        public static readonly DependencyProperty FontStretchProperty =
            DependencyProperty.Register("FontStretch", typeof(FontStretch), typeof(AxisControl),
                new PropertyMetadata(FontStretches.Normal));

        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }
        #endregion

        #region TickLabelFontSize
        public static readonly DependencyProperty TickLabelFontSizeProperty =
            DependencyProperty.Register("TickLabelFontSize", typeof(double), typeof(AxisControl),
                new PropertyMetadata(11.0));

        public double TickLabelFontSize
        {
            get { return (double)GetValue(TickLabelFontSizeProperty); }
            set { SetValue(TickLabelFontSizeProperty, value); }
        }
        #endregion


        protected override Size MeasureOverride(Size availableSize)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Axis type={0}", this.GetType()));

            Rect rect = Rect.Empty;
            //Rect rect1 = Rect.Empty;
            foreach (Visual visual in visuals)
            {
                IRenderBound axisVisual = visual as IRenderBound;
                if (axisVisual != null)
                {
                    rect.Union(axisVisual.GetRenderBound());
                }
                //DrawingVisual dv = visual as DrawingVisual;
                //if(dv != null)
                //{
                //    rect1.Union(dv.ContentBounds);
                //    rect1.Union(dv.DescendantBounds);
                //}
            }
            //System.Diagnostics.Debug.WriteLine(string.Format("rect1 : l={0},r={1},t={2},b={3},w={4},h={5}", rect1.Left, rect1.Right, rect1.Top, rect1.Bottom, rect1.Width, rect1.Height));
            System.Diagnostics.Debug.WriteLine(string.Format("rect : l={0},r={1},t={2},b={3},w={4},h={5}", rect.Left, rect.Right, rect.Top, rect.Bottom, rect.Width, rect.Height));
            if (rect.IsEmpty || double.IsInfinity(rect.Width) || double.IsInfinity(rect.Height))
            {
                return new Size(0, 0);
            }
            return new Size(rect.Width, rect.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return finalSize;
        }

        public virtual void Update()
        {
            RenderVisuals();
            InvalidateVisual();
            InvalidateMeasure();
        }

        protected virtual void RenderVisuals()
        {
            foreach (Visual visual in visuals)
            {
                IAxisRender axisVisual = visual as IAxisRender;
                System.Diagnostics.Debug.Assert(axisVisual != null, "axisVisual != null");
                axisVisual.Render(GetAxisDataContext());
            }
        }

        protected virtual AxisVisualContext GetAxisDataContext()
        {
            return new AxisVisualContext();
        }
    }
}
