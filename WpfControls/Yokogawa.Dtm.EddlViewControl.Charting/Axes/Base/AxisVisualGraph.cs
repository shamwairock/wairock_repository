using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public abstract class AxisVisualGraph : IAxisRender, IRenderBound
    {
        public virtual IAxisVisualModel AxisModel { get; set; }

        public abstract void Render(AxisVisualContext dataContext);

        public abstract Rect GetRenderBound();
    }
}
