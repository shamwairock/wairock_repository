
namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public enum LogarithmicAxisTicks
    {
        /// <summary>
        /// Tick at base pixels 2 is visible
        /// </summary>
        Two = 0x1,
        /// <summary>
        /// Tick at base pixels 3 is visible
        /// </summary>
        Three = 0x2,
        /// <summary>
        /// Tick at base pixels 4 is visible
        /// </summary>
        Four = 0x4,
        /// <summary>
        /// Tick at base pixels 5 is visible
        /// </summary>
        Five = 0x8,
        /// <summary>
        /// Tick at base pixels 6 is visible
        /// </summary>
        Six = 0x10,
        /// <summary>
        /// Tick at base pixels 7 is visible
        /// </summary>
        Seven = 0x20,
        /// <summary>
        /// Tick at base pixels 8 is visible
        /// </summary>
        Eight = 0x40,
        /// <summary>
        /// Tick at base pixels 9 is visible
        /// </summary>
        Nine = 0x80,
        /// <summary>
        /// Ticks at base values 2 and 5 are visible
        /// </summary>
        Main = Two | Five,
        /// <summary>
        /// All Ticks visible
        /// </summary>
        All = Two | Three | Four | Five | Six | Seven | Eight | Nine,
    }
}
