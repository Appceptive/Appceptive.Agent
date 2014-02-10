using System;

namespace Appceptive.Agent.Core.Logging
{
    public interface ILogger
    {
        void Debug(string format, params object[] args);
        void Information(string format, params object[] args);
        void Warning(string format, params object[] args);
        void Error(string format, params object[] args);
        void Error(Exception ex, string format, params object[] args);
    }
}