namespace CSharpCodeSamples.Domain.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;

    using Common.Interfaces.Models;
    using Common.Logging;
    using Models;

    public class TPGTraceListener : TraceListener
    {
        private readonly List<ActivityLogException> _errorStack = new List<ActivityLogException>();

        /// <summary>
        /// Writes the specified message to the configured listener.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void Write(string message)
        {
            message = message.Trim();
            LogItemSeverity severity = LogItemSeverity.Error;
            if (message.StartsWith("INFO", StringComparison.InvariantCultureIgnoreCase))
            {
                message = message.Substring(4);
                severity = LogItemSeverity.Info;
            }
            else if (message.StartsWith("WARNING", StringComparison.InvariantCultureIgnoreCase))
            {
                message = message.Substring(7);
                severity = LogItemSeverity.Warning;
            }
            _errorStack.Add(new ActivityLogException(severity, message));
        }

        public override void WriteLine(string message)
        {
            if (!String.IsNullOrEmpty(message))
                Write(message + "\n");
        }

        /// <summary>
        /// Adds the supplied exception to the error log cache.
        /// </summary>
        /// <param name="o">
        /// If object is valid <seealso ref="ActivityLogException"/>
        /// will add exception to the error log cache.
        /// Otherwise does nothing.
        /// </param>
        public override void WriteLine(object o)
        {
            ActivityLogException e = o as ActivityLogException;
            if (e != null) _errorStack.Add(e);
        }

        public override void Close()
        {
            base.Close();
            SaveLogData();
        }
        public override void Flush()
        {
            base.Flush();
            SaveLogData();
        }

        /// <summary>
        /// Clears the error log cache,
        /// writing each error to the db activity log.
        /// </summary>
        private void SaveLogData()
        {
            foreach (ActivityLogException err in _errorStack)
                PostErrorToDB(err);
            _errorStack.Clear();
        }

        /// <summary>
        /// Writes a single error entry to the db activity log.
        /// </summary>
        /// <param name="item">
        /// The exception containing information to be posted
        /// to the error log.
        /// </param>
        /// <remarks>
        /// Lots more can be done here, this is just a small initial implementation
        /// </remarks>
        private static void PostErrorToDB(ActivityLogException item)
        {
            if (item == null) return;

            List<SqlParameter> parms = new List<SqlParameter>
            {
                new SqlParameter("@Timestamp", SqlDbType.DateTime)
                {
                    Direction = ParameterDirection.Input,
                    SqlValue = item.EventTimestamp
                },
                new SqlParameter("@Severity", SqlDbType.SmallInt)
                {
                    Direction = ParameterDirection.Input,
                    SqlValue = (int)item.Severity
                },
                new SqlParameter("@BrowserUserAgent", SqlDbType.VarChar, 50)
                {
                    Direction = ParameterDirection.Input,
                    SqlValue = item.BrowserUserAgent
                },
                new SqlParameter("@WebServerName", SqlDbType.VarChar, 100)
                {
                    Direction = ParameterDirection.Input,
                    SqlValue = item.WebServerName
                },
                new SqlParameter("@UserName", SqlDbType.VarChar, 50)
                {
                    Direction = ParameterDirection.Input,
                    SqlValue = item.User
                },
                new SqlParameter("@Source", SqlDbType.VarChar, 2000)
                {
                    Direction = ParameterDirection.Input,
                    SqlValue = item.Source ?? ""
                },
                new SqlParameter("@Procedure", SqlDbType.VarChar, 100)
                {
                    Direction = ParameterDirection.Input,
                    SqlValue = item.Procedure ?? ""
                },
                new SqlParameter("@Message", SqlDbType.VarChar, 2000)
                {
                    Direction = ParameterDirection.Input,
                    SqlValue = item.Message
                },
                new SqlParameter("@Comment", SqlDbType.VarChar)
                {
                    Direction = ParameterDirection.Input,
                    SqlValue = item.Comment
                },
                new SqlParameter("@StackTrace", SqlDbType.VarChar)
                {
                    Direction = ParameterDirection.Input,
                    SqlValue = (item.InnerException != null && !String.IsNullOrEmpty(item.InnerException.StackTrace))
                        ? item.InnerException.StackTrace
                        : (!String.IsNullOrWhiteSpace(item.StackTrace) ? item.StackTrace : "")
                }
            };

            IUserContext ucon = new UserContext();
            ucon.InitializeValues(item.BrowserUserAgent, "", item.User);

            //write command removed - proprietary
            //place database write command (call to ef or ado.net) here
        }
    }
}
