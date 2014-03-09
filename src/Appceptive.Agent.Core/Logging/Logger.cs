using System;

namespace Appceptive.Agent.Core.Logging
{
    public class Logger
    {
        public static ILogger Current { get; set; }

        public static void Debug(string format, params object[] args)
        {
            Current.Debug(format, args);
        }

        public static void Information(string format, params object[] args)
        {
            Current.Information(format, args);
        }

        public static void Warning(string format, params object[] args)
        {
            Current.Warning(format, args);
        }

        public static void Error(string format, params object[] args)
        {
            Current.Error(format, args);
        }

        public static void Error(Exception ex, string format, params object[] args)
        {
            Current.Error(ex, format, args);
        }
    }
}