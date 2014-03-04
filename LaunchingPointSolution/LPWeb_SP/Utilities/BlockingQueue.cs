using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Utilities
{
    #region BlockingQueue
    public class BlockingQueue<T>
    {
        private List<T> q = new List<T>();

        public event EventHandler ListChanged;

        public void Enqueue(T element)
        {
            q.Add(element);
            lock (q)
            {
                Monitor.Pulse(q);
            }
            if (ListChanged != null)
            {
                ListChanged(this, null);
            }
        }

        public int getCount()
        {
            lock (q)
            {
                return q.Count;
            }
        }

        public T DequeueNonblocking()
        {
            T element = default(T);
            lock (q)
            {
                if (q.Count > 0)
                {
                    element = q[0];
                    q.RemoveAt(0);
                }
            }
            if (ListChanged != null)
            {
                ListChanged(this, null);
            }

            return element;
        }

        public void Clear()
        {
            lock (q)
            {
                q.Clear();
            }
        }

        public bool Remove(T call)
        {
            lock (q)
            {
                if (q.Count > 0)
                {
                    q.Remove(call);
                    if (ListChanged != null)
                    {
                        ListChanged(this, null);
                    }
                }
            }

            return false;
        }

        public List<T> GetItems()
        {
            List<T> list = new List<T>();
            lock (q)
            {
                foreach (T item in q)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public T Peek()
        {
            T element = default(T);
            lock (q)
            {
                if (q.Count > 0)
                {
                    element = q[0];
                }
            }

            return element;
        }


        public T Dequeue()
        {
            lock (q)
            {
                while (q.Count == 0)
                {
                    Monitor.Wait(q);
                }

                T element = q[0];
                q.RemoveAt(0);

                if (ListChanged != null)
                {
                    ListChanged(this, null);
                }

                return element;
            }
        }
    };
    #endregion
}
