using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

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

        public void Error(Exception exception)
        {
            Trace.WriteLine(FlattenException(exception), DateTime.Now.ToString(DATETIME_FORMAT) + " ERROR");
        }

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

        public static string FlattenException(Exception exception)
        {
            var stringBuilder = new StringBuilder();

            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.StackTrace);

                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        }
    }
}
