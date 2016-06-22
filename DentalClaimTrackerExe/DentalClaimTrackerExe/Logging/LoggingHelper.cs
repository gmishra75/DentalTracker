using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;
using System.Windows.Forms;

namespace C_DentalClaimTracker
{
    public enum LogSeverity
    {
        Verbose = TraceEventType.Verbose,
        Information = TraceEventType.Information,
        Warning = TraceEventType.Warning,
        Error = TraceEventType.Error,
        Critical = TraceEventType.Critical
    }    
    
    public static class LoggingHelper
    {
        static bool _showErrors = true;
        /// <summary>
        /// Automatically appends current time and user
        /// </summary>
        /// <param name="message"></param>
        /// <param name="severity"></param>
        public static void Log(string message, LogSeverity severity, Exception ex = null, bool throwException = false)
        {
            try
            {
                var entry = new LogEntry();

                entry.Severity = (TraceEventType)severity;
                entry.Message = GetUser() + message;

                if (ex != null)
                {
                    entry.AddErrorMessage(ex.Message);
                    Exception e = ex.InnerException;
                    while (e != null)
                    {
                        entry.AddErrorMessage(e.Message);
                        e = e.InnerException;
                    }
                }

                Logger.Write(entry);

                
            }
            catch
            {
                if (_showErrors)
                {
                    //MessageBox.Show("An error occurred attempting to log a previous error. Your error logging might not be configured correctly. Please notify a system administrator of this problem.",
                    //    "Could not log error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _showErrors = false;

                }
            }

            if (throwException)
                throw ex;
        }

        private static string GetUser()
        {
            string extraMessage;
            
            try
            {
                extraMessage = ActiveUser.UserObject.username;
            }
            catch { extraMessage = "No user"; }
            return extraMessage + " ";
        }

        public static void Log(Exception ex, bool throwException = false)
        {
            try
            {
                var entry = new LogEntry();
                entry.Severity = TraceEventType.Error;

                entry.Message = GetUser() + ex.Message;

                Exception inner = ex.InnerException;
                while (inner != null)
                {
                    entry.AddErrorMessage(inner.Message);
                    inner = inner.InnerException;
                }

                Logger.Write(entry);

                if (throwException)
                    throw ex;
            }
            catch
            {
                if (_showErrors)
                {
                    MessageBox.Show("An error occurred attempting to log a previous error. Your error logging might not be configured correctly. Please notify a system administrator of this problem.",
                        "Could not log error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _showErrors = false;

                }
            }
        }
    }
}
