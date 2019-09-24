using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Event_Handling_with_Inheritance
{
    public class Children : Parent
    {
        public void TestRaiseEvent()
        {
            OnRaiseSubscribeEvent(null, new EventArgs());
        }
    }
}
