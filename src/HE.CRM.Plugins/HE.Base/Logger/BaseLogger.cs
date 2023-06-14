using System;
using Microsoft.Xrm.Sdk;

namespace HE.Base.Log
{
    public class BaseLogger : IBaseLogger
    {
        public LogLevel LogLevel { get; private set; }

        private readonly ITracingService tracingService;

        public BaseLogger(ITracingService tracingService, LogSettings settings)
        {
            this.tracingService = tracingService;
            this.LogLevel = settings.Level;
        }

        public void Debug(string message)
        {
            InternalLog(LogLevel.Debug, message);
        }

        public void Error(string message)
        {
            InternalLog(LogLevel.Error, message);
        }

        public void Fatal(string message)
        {
            InternalLog(LogLevel.Fatal, message);
        }

        public void Info(string message)
        {
            InternalLog(LogLevel.Info, message);
        }

        public void Trace(string message)
        {
            InternalLog(LogLevel.Trace, message);
        }

        public void Warn(string message)
        {
            InternalLog(LogLevel.Warn, message);
        }

        private void InternalLog(LogLevel logLevel, string message)
        {
            if (LogLevel <= logLevel)
                tracingService.Trace($"{DateTime.Now:hh:mm:ss.fff} [{logLevel.ToString().ToLower()}]: {message}");
        }
    }
}
