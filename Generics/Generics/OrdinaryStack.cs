using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Generics
{
    public class OrdinaryStack
    {
        private object[] arrayObjects = null;
        private int pointer = 0;

        public OrdinaryStack()
        {
            arrayObjects = new object[0];
        }

        public object Peek()
        {
            if (arrayObjects != null && arrayObjects.Length != 0)
            {
                return arrayObjects[pointer - 1];
            }
            else
            {
                throw new NullReferenceException("No item in the Stack");
            }
        }

        public object Pop()
        {
            if (arrayObjects != null && arrayObjects.Length != 0)
            {
                var item = arrayObjects[pointer - 1];
                pointer--;

                Array.Clear(arrayObjects, pointer, 1);
                Array.Resize(ref arrayObjects, pointer);

                return item;
            }
            else
            {
                throw new NullReferenceException("No item in the Stack");
            }
        }

        public void Push(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (arrayObjects != null)
            {
                Array.Resize(ref arrayObjects, arrayObjects.Length + 1);
                arrayObjects[pointer] = item;
                pointer++;
            }
        }
    }
}
