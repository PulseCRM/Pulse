using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Common
{
    class Log
    {
        static TextWriterTraceListener tr1;

        public void Init(string path)
        {
            tr1 = new TextWriterTraceListener(path + this.GetType() + ".log");
            Trace.Listeners.Add(tr1);
        }

        public void Debug(object sender, string msg)
        {
            string msg1 = DateTime.Now.ToString() + " " + sender.GetType().ToString() + ": " + msg;
            Trace.TraceInformation(msg1);
            Trace.Flush();
        }

        public void Warn(object sender, string msg)
        {
            string msg1 = DateTime.Now.ToString() + " " + sender.GetType().ToString() + ": " + msg;
            Trace.TraceWarning(msg1);
        }

        public void Error(object sender, string msg)
        {          
            string msg1 = DateTime.Now.ToString() + " " + sender.GetType().ToString() + ": " + msg;
            Trace.TraceError(msg1);
        }
    }
}
