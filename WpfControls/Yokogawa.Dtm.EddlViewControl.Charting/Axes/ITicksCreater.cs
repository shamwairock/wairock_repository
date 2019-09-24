using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public interface ITicksCreater
    {
        TickInfo[] GetTicks(int ticksCount);

        int DecreaseTickCount(int tickCount);

        int IncreaseTickCount(int tickCount);

        int DefaultTickCount();

        ITicksCreater MinorTickCreater { get; }
    }

    public interface ITicksCreater<T> : ITicksCreater
    {
        TickInfo[] GetTicks(T start, T stop, int ticksCount);
    }
}
