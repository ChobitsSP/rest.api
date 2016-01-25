using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Rest.Api.Util
{
    public class FileLogger
    {
        //const string LOG_FILE_NAME = "service.log";
        const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public FileLogger(string filePath)
        {
            try
            {
                Trace.Listeners.Add(new TextWriterTraceListener(filePath));
            }
            catch //(Exception e)  
            {
                Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            }
            Trace.AutoFlush = true;
        }

        #region interface  

        public void Error(string message)
        {
            Trace.WriteLine(message, DateTime.Now.ToString(DATETIME_FORMAT) + " ERROR");
        }

        public void Warn(string message)
        {
            Trace.WriteLine(message, DateTime.Now.ToString(DATETIME_FORMAT) + " WARN");
        }

        public void Info(string message)
        {
            Trace.WriteLine(message, DateTime.Now.ToString(DATETIME_FORMAT) + " INFO");
        }

        #endregion

    }
}
