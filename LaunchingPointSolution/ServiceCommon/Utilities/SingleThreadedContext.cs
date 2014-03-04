using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Utilities
{
    public class SendPostDelegateItem
    {
        public SendOrPostCallback callback;
        public object param;

        public SendPostDelegateItem(SendOrPostCallback spb, object p)
        {
            callback = spb;
            param = p;
        }
    }

    public delegate void ExceptionEventHandler(Object sender, ExceptionEventArgs e);

    public class ExceptionEventArgs : EventArgs
    {
        Exception m_ex;

        public Exception Exception
        {
            get { return m_ex; }
        }

        public ExceptionEventArgs(Exception ex)
        {
            m_ex = ex;
        }

    }


    public class SingleThreadedContext
    {
        SendPostDelegateItem m_exitThreadItem = new SendPostDelegateItem(null, null);
        private BlockingQueue<SendPostDelegateItem> m_queue = new BlockingQueue<SendPostDelegateItem>();

        Thread m_executorThread = null;
        bool m_lock = false;

        public event ExceptionEventHandler ExceptionEvent = null;

        public void Init()
        {
            Init("SingleThreadedContext");
        }

        public void Init(string name)
        {
            try
            {
                m_executorThread = new Thread(new ParameterizedThreadStart(ExecutorThreadRun));
                m_executorThread.Name = name;
                m_executorThread.IsBackground = true;
                m_executorThread.Start(m_queue);
            }
            catch (Exception ex)
            {
                //LogWriter.Write(LogLevel.Fatal, "Failed to start " + m_executorThread.Name + " thread", ex);
            }
        }

        public void Exit()
        {
            m_queue.Enqueue(m_exitThreadItem);
        }

        public int Count()
        {
            return m_queue.getCount();
        }

        public void Lock()
        {
            lock (this)
            {
                m_lock = true;
            }
        }

        public void Unlock()
        {
            lock (this)
            {
                m_lock = false;
            }
        }

        public void Post(SendOrPostCallback callback, object param)
        {
            lock (this)
            {
                if (!m_lock)
                {
                    m_queue.Enqueue(new SendPostDelegateItem(callback, param));
                }
            }
        }

        public void ClearQueue()
        {
            m_queue.Clear();
        }

        private void ExecutorThreadRun(object queue)
        {
            BlockingQueue<SendPostDelegateItem> q = queue as BlockingQueue<SendPostDelegateItem>;
            SendPostDelegateItem item = null;
            if (q == null)
                return;

            do
            {
                try
                {
                    if (q == null)
                    {
                        //LogWriter.Write(LogLevel.Fatal, System.Reflection.MethodBase.GetCurrentMethod()+" " + m_executorThread.Name + " Queue NULL");
                        return;
                    }

                    item = q.Dequeue();

                    if (item == m_exitThreadItem)
                    {
                        //LogWriter.Write(LogLevel.Info, "Exiting " + m_executorThread.Name);
                        break;
                    }
                    try
                    {
                        item.callback(item.param);
                    }
                    catch (Exception ex)
                    {
                        //LogWriter.Write(LogLevel.Fatal, " Executor exception", ex);
                        ExceptionEventArgs args = new ExceptionEventArgs(ex);
                        if (ExceptionEvent != null)
                        {
                            ExceptionEvent(item, args);
                        }
                    }
                }
                catch (Exception ex1)
                {
                    try
                    {
                        //LogWriter.Write(LogLevel.Fatal, System.Reflection.MethodBase.GetCurrentMethod()+" " + m_executorThread.Name + " exception", ex1);
                    }
                    catch (Exception ex2)
                    {
                        System.Diagnostics.Debug.WriteLine(System.Reflection.MethodBase.GetCurrentMethod() + " " + m_executorThread.Name + "LogWriter threw exception " + ex2);
                        return;
                    }

                    if (q == null)
                    {
                        //LogWriter.Write(LogLevel.Fatal, System.Reflection.MethodBase.GetCurrentMethod() + " "+m_executorThread.Name + " Queue NULL", ex1);
                        break;
                    }
                }

            } while (true);
        }
    }
}
