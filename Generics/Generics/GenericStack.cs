using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Generics
{
    public class GenericStack<T>
    {
        private T[] arrayObjects = null;
        private int pointer = 0;

        public GenericStack()
        {
            arrayObjects = new T[0];
        }

        public T Peek()
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

        public T Pop()
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

        public void Push(T item)
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
