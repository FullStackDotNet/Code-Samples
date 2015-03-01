namespace CSharpCodeSamples.Common.Interfaces.Logging
{
    using System;

    using Common.Logging;
    using Models;

    /// <summary>
    /// Interface ILogger
    /// </summary>
    public interface ILogger
    {
        void Info(string message, IUserContext ucon = null);
        void Info(ActivityLogException exception);

        void Warning(string message, IUserContext ucon = null);
        void Warning(ActivityLogException exception);

        void Error(string message, IUserContext ucon = null);
        void Error(ActivityLogException exception);

        void TraceApi(string componentName, string method, TimeSpan timespan);
        void TraceApi(string componentName, string method, TimeSpan timespan, string properties);
    }
}
