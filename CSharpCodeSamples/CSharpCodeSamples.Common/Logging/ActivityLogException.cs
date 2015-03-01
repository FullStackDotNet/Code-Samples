namespace CSharpCodeSamples.Common.Logging
{
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics;

    using Interfaces.Models;

    public enum ErrorType
    {
        Error,
        Other,
        DeletedItemInUse,
        Duplicate,
        NoRecords
    }

    public enum LogItemSeverity
    {
        Info,
        Warning,
        Error
    }

    /// <summary>
    /// A simple <seealso cref="Exception"/> subclass used to track additional data related to an exception for logging purposes.
    /// </summary>
    [Serializable]
    public class ActivityLogException : Exception
    {
        public DateTime        EventTimestamp     { get; private set; }
        public LogItemSeverity Severity           { get; set; }
        public string          BrowserUserAgent   { get; private set; }
        public string          WebServerName      { get; private set; }
        public string          Procedure          { get; private set; }
        public ErrorType       Result             { get; private set; }
        public string          User               { get; private set; }
        public string          Comment            { get; set; }

        public ActivityLogException(LogItemSeverity severity, string message)
            : base(message)
        {
            EventTimestamp     = DateTime.Now;
            Severity = severity;
            BrowserUserAgent   = "";
            WebServerName      = Environment.MachineName;
            base.Source        = String.Empty;
            Procedure          = String.Empty;
            User               = "";
            Comment            = "";
        }


        public ActivityLogException(string message, IUserContext request)
            : base(message)
        {
            Debug.Assert(request != null);

            EventTimestamp     = DateTime.Now;
            BrowserUserAgent   = request.BrowserAgent;
            WebServerName      = Environment.MachineName;
            base.Source        = String.Empty;
            Procedure          = String.Empty;
            User               = request.UserName;
            Comment            = request.SourceURL;
        }

        public ActivityLogException(SqlException e, ErrorType et, IUserContext request)
            : base(e.Message, e)
        {
            EventTimestamp     = DateTime.Now;
            Severity           = LogItemSeverity.Error;
            BrowserUserAgent   = request == null ? "" : request.BrowserAgent;
            WebServerName      = Environment.MachineName;
            base.Source        = e.Source;
            Procedure          = e.Procedure;
            Result             = et;
            User               = request == null ? "" : request.UserName;
            Comment            = request == null ? "" : request.SourceURL;
        }

        public ActivityLogException(Exception e, IUserContext request)
            : base(e.Message, e)
        {
            EventTimestamp     = DateTime.Now;
            Severity           = LogItemSeverity.Error;
            BrowserUserAgent   = request == null ? "" : request.BrowserAgent;
            WebServerName      = Environment.MachineName;
            base.Source        = e.Source;
            Procedure          = String.Empty;
            Result             = ErrorType.Other;
            User               = request == null ? "" : request.UserName;
            Comment            = request == null ? "" : request.SourceURL;
        }
    }
}
