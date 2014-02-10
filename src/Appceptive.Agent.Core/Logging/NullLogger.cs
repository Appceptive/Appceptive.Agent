using System;

namespace Appceptive.Agent.Core.Logging
{
    public class NullLogger : ILogger
    {
        public void Debug(string format, params object[] args)
        {
        }

        public void Information(string format, params object[] args)
        {
        }

        public void Warning(string format, params object[] args)
        {
        }

        public void Error(string format, object[] args)
        {
        }

        public void Error(Exception ex, string format, object[] args)
        {
        }
    }
}