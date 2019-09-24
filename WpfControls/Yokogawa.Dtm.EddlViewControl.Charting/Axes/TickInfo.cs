
namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public struct TickInfo
    {
        public TickInfo(object value, double tickPos,bool isLongTick, double tickLength)
        {
            this.value = value;
            this.isLong = isLongTick;
            this.tickPos = tickPos;
            this.tickLength = tickLength;
        }

        private readonly object value;
        public object Value { get { return value; } }

        private readonly bool isLong;
        public bool IsLong { get { return isLong; } }

        private readonly double tickPos;
        public double TickPos { get { return tickPos; } }

        private readonly double tickLength;
        public double TickLength { get { return tickLength; } }
    }
}
