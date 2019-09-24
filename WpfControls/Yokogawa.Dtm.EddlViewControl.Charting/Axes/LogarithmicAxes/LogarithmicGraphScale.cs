using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    internal class LogarithmicGraphScale
    {
        private double scale = 1;

        private double minimum = 1;
        public double Minimum { get { return minimum; } set { minimum = value; } }

        private double maximum = 1000;
        public double Maximum { get { return maximum; } set { maximum = value; } }

        private double startPosistion = 200;
        public double StartPosistion { get { return startPosistion; } set { startPosistion = value; } }

        private double stopPosistion = 0;
        public double StopPosistion { get { return stopPosistion; } set { stopPosistion = value; } }

        public void Update()
        {
            scale = (StopPosistion - StartPosistion) / Math.Log10(Maximum / Minimum);
        }

        public double GetPositionByValue(double value)
        {
            return StartPosistion + Math.Log10(value / Minimum) * scale;
        }

        public double GetValueByPoistion(double postion)
        {
            return Minimum * Math.Pow(10, (postion - StartPosistion) / scale);
        }
    }
}
