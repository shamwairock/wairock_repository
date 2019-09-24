using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class LogarithmicTickCreator
    {
        public static bool CanUseLogarithmicTick(Range<double> range)
        {
            var tickMinPos = LogarithmicTickCalculator.AdjacentTick(range.Min, false);
            var tickMaxPos = LogarithmicTickCalculator.AdjacentTick(range.Max, false);
            return tickMaxPos.Value != tickMinPos.Value;
        }

        private LogarithmicTickCalculator logarithmicTickCalculator = null;
        public void SetRange(Range<double> range)
        {
            tickMapIndex = 0;
            tickAlternate = 0;

            logarithmicTickCalculator = new LogarithmicTickCalculator();
            logarithmicTickCalculator.SetRange(range);
        }

        public LogarithmicTick[] CreateTicks(bool only_inside = true)
        {
            if (logarithmicTickCalculator != null)
            {
                var ret = new List<LogarithmicTick>();
                var values = logarithmicTickCalculator.CreateTicks(only_inside);
                if (values != null && values.Length > 0)
                {
                    if (tickAlternate == 0)
                    {
                        FilterSubticks(ret, values);
                    }
                    else
                    {
                        FilterMainTicks(ret, values);
                    }
                }
                return ret.ToArray();
            }
            return null;
        }

        private void FilterMainTicks(List<LogarithmicTick> ret, LogarithmicValue[] values)
        {
            var mainTicks = (from v in values
                             where v.A == 1.0
                             select v).ToArray();
            int flag = 0;
            foreach (var v in values)
            {
                var tick = new LogarithmicTick()
                {
                    Value = v,
                    LabelVisible = false,
                    TickVisible = true,
                    LongTick = v.A == 1,
                };
                if (v.A == 1.0)
                {
                    if (flag % (tickAlternate + 1) == 0)
                    {
                        tick.LabelVisible = true;
                    }
                    flag++;
                }
                ret.Add(tick);
            }
        }

        private void FilterSubticks(List<LogarithmicTick> ret, LogarithmicValue[] values)
        {
            var map = tickMapArray[tickMapIndex];
            foreach (var v in values)
            {
                var tick = new LogarithmicTick()
                {
                    Value = v,
                    LabelVisible = map.Contains(v.A),
                    TickVisible = true,
                    LongTick = v.A == 1,
                };
                ret.Add(tick);
            }
        }

        private int tickAlternate = 0;
        private int tickMapIndex = 0;
        public List<double[]> tickMapArray = new List<double[]>()
        {
            new double[]{ 1,2,3,4,5,6,7,8,9 },
            new double[]{ 1,2,4,6,8 },
            new double[]{ 1,2,4},
            new double[]{ 1,3},
            new double[]{ 1},
        };

        public void DescentTickCount()
        {
            if (tickMapIndex == tickMapArray.Count - 1)
            {
                tickAlternate++;
            }
            else
            {
                tickMapIndex++;
                if (tickMapIndex >= tickMapArray.Count)
                {
                    tickMapIndex = tickMapArray.Count - 1;
                }
            }
        }

        public void IncreaseTickCount()
        {
            if (tickAlternate == 0)
            {
                if (tickMapIndex != 0)
                {
                    tickMapIndex--;
                    if (tickMapIndex < 0)
                    {
                        tickMapIndex = 0;
                    }
                }
            }
            else
            {
                tickAlternate--;
                if (tickAlternate == 0)
                {
                    tickMapIndex = tickMapArray.Count - 1;
                }
            }
        }
    }
}
