using System;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public enum DateTimeTickUnits
    {
        /// <summary>
        /// DateTime Scale is measured in <see cref="DateTime"/> Ticks.
        /// </summary>
        Ticks,
        /// <summary>
        /// DateTime Scale is measured in Milliseconds.
        /// </summary>
        Milliseconds,
        /// <summary>
        /// DateTime Scale is measured in Seconds.
        /// </summary>
        Seconds,
        /// <summary>
        /// DateTime Scale is measured in Minutes.
        /// </summary>
        Minutes,
        /// <summary>
        /// DateTime Scale is measured in Hours.
        /// </summary>
        Hours,
        /// <summary>
        /// DateTime Scale is measured in Days.
        /// </summary>
        Days,
        /// <summary>
        /// DateTime Scale is measured in Weeks.
        /// </summary>
        Weeks,
        /// <summary>
        /// DateTime Scale is measured in Months.
        /// </summary>
        Months,
        /// <summary>
        /// DateTime Scale is measured in Years.
        /// </summary>
        Years
    }
}
