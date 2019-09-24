using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public interface IAxisRender
    {
        void Render(AxisVisualContext dataContext);
    }

    public class AxisVisualContext
    {
        public IDrawingContextProvider Render { get; set; }
    }

    public interface IDrawingContextProvider
    {
        DrawingContext AxisRenderOpen();
    }
}
