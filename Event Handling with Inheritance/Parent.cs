using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Event_Handling_with_Inheritance
{
    public abstract class Parent
    {
        protected delegate void RaiseSubscribeEventHandler(object obj, EventArgs eventArgs);
        protected event RaiseSubscribeEventHandler RaiseSubscribeEvent;

        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        protected virtual void OnRaiseSubscribeEvent(object obj, EventArgs eventArgs)
        {
            RaiseSubscribeEvent?.Invoke(obj, eventArgs);
        }
    }
}
