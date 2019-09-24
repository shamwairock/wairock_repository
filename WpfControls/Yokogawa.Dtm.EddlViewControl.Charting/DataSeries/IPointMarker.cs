using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public interface IPointMarker
    {
        Drawing PointMarker { get; set; }

        bool PointMarkerVisible { get; set; }

        Drawing EmphasisPointMarker { get; set; }
    }
}
