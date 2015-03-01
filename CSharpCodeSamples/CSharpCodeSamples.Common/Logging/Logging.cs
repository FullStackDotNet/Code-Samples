namespace CSharpCodeSamples.Common.Logging
{
    using System;
    using System.Diagnostics;

    using Common;
    using Interfaces.Logging;
    using Interfaces.Models;

    /// <summary>
    /// Logger provides convenience methods for creating log entries.
    /// </summary>
    public class Logger : ILogger
    {
        public void Info(string message, IUserContext ucon = null)
        {
            ActivityLogException exception = (ucon == null)
                                                ? new ActivityLogException(LogItemSeverity.Info, message)
                                                : new ActivityLogException(message, ucon);
            exception.Severity = LogItemSeverity.Info;
            Trace.WriteLineIf(Constants.SystemTraceLevel.TraceInfo, exception);
        }

        public void Info(ActivityLogException exception)
        {
            exception.Severity = LogItemSeverity.Info;
            Trace.WriteLineIf(Constants.SystemTraceLevel.TraceInfo, exception);
        }

        public void Warning(string message, IUserContext ucon = null)
        {
            ActivityLogException exception = (ucon == null)
                                                ? new ActivityLogException(LogItemSeverity.Warning, message)
                                                : new ActivityLogException(message, ucon);
            exception.Severity = LogItemSeverity.Warning;
            Trace.WriteLineIf(Constants.SystemTraceLevel.TraceWarning, exception);
        }

        public void Warning(ActivityLogException exception)
        {
            exception.Severity = LogItemSeverity.Warning;
            Trace.WriteLineIf(Constants.SystemTraceLevel.TraceWarning, exception);
        }

        public void Error(string message, IUserContext ucon = null)
        {
            ActivityLogException exception = (ucon == null)
                                                ? new ActivityLogException(LogItemSeverity.Error, message)
                                                : new ActivityLogException(message, ucon);
            exception.Severity = LogItemSeverity.Error;
            Trace.WriteLineIf(Constants.SystemTraceLevel.TraceError, exception);
        }

        public void Error(ActivityLogException exception)
        {
            exception.Severity = LogItemSeverity.Error;
            Trace.WriteLineIf(Constants.SystemTraceLevel.TraceError, exception);
        }

        public void TraceApi(string componentName, string method, TimeSpan timespan)
        {
            TraceApi(componentName, method, timespan, "");
        }

        public void TraceApi(string componentName, string method, TimeSpan timespan, string properties)
        {
            string message = String.Concat("Component:", componentName, ";Method:", method, ";Timespan:", timespan.ToString(), ";Properties:", properties);
            Trace.WriteLineIf(Constants.SystemTraceLevel.TraceVerbose, message);
        }
    }
}
