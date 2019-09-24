using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class DataList<T> : IList, IList<T>
    {
        private readonly object lockobject = new object();
        private List<T> source = null;

        public DataList()
        {
            source = new List<T>();
        }

        public T this[int index]
        {
            get
            {
                lock (lockobject)
                {
                    return source[index];
                }
            }
            set
            {
                lock (lockobject)
                {
                    source[index] = value;
                }
            }
        }

        object IList.this[int index]
        {
            get
            {
                lock (lockobject)
                {
                    return source[index];
                }
            }
            set
            {
                lock (lockobject)
                {
                    source[index] = (T)value;
                }
            }
        }

        public int Count { get { return source.Count; } }

        public bool IsReadOnly { get { return ((IList<T>)source).IsReadOnly; } }

        public bool IsFixedSize { get { return ((IList)source).IsFixedSize; } }

        public object SyncRoot { get { return ((IList)source).SyncRoot; } }

        public bool IsSynchronized { get { return ((IList)source).IsSynchronized; } }

        public void Add(T item)
        {
            lock (lockobject)
            {
                source.Add(item);
            }
        }

        public int Add(object value)
        {
            var v = (T)value;
            Add(v);
            lock (lockobject)
            {
                return source.IndexOf(v);
            }
        }

        public void Clear()
        {
            lock (lockobject)
            {
                source.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (lockobject)
            {
                return source.Contains(item);
            }
        }

        public bool Contains(object value)
        {
            var v = (T)value;
            return Contains(v);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (lockobject)
            {
                source.CopyTo(array, arrayIndex);
            }
        }

        public void CopyTo(Array array, int index)
        {
            CopyTo(array.Cast<T>().ToArray(), index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (lockobject)
            {
                return source.GetEnumerator();
            }
        }

        public int IndexOf(T item)
        {
            lock (lockobject)
            {
                return source.IndexOf(item);
            }
        }

        public int IndexOf(object value)
        {
            return IndexOf((T)value);
        }

        public void Insert(int index, T item)
        {
            lock (lockobject)
            {
                source.Insert(index, item);
            }
        }

        public void Insert(int index, object value)
        {
            Insert(index, (T)value);
        }

        public bool Remove(T item)
        {
            lock (lockobject)
            {
                return source.Remove(item);
            }
        }

        public void Remove(object value)
        {
            Remove((T)value);
        }

        public void RemoveAt(int index)
        {
            lock (lockobject)
            {
                source.RemoveAt(index);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
