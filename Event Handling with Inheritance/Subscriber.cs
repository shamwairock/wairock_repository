using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Event_Handling_with_Inheritance
{
    public class Subscriber
    {
        
        public Subscriber()
        {
            Children children = new Children();
            children.TestRaiseEvent();
        }
    }
}
