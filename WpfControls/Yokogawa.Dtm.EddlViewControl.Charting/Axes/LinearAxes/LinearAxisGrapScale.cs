using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    internal class LinearAxisGrapScale
    {
        private double scale = 1;

        private double minimum = 0;
        public double Minimum { get { return minimum; } set { minimum = value; } }

        private double maximum = 100;
        public double Maximum { get { return maximum; } set { maximum = value; } }

        private double startPosistion = 200;
        public double StartPosistion { get { return startPosistion; } set { startPosistion = value; } }

        private double stopPosistion = 0;
        public double StopPosistion { get { return stopPosistion; } set { stopPosistion = value; } }

        public void Update()
        {
            scale = (Maximum - Minimum) / (StopPosistion - StartPosistion);
        }

        public double GetPositionByValue(double value)
        {
            return StartPosistion + (value - Minimum) / scale;
        }

        public double GetValueByPoistion(double position)
        {
            return Minimum + (position - StartPosistion) * scale;
        }
    }
}
