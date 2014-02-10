using System;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Appceptive.Agent.Serilog
{
    public static class AppceptiveLoggerConfigurationExtensions
    {        
        public static LoggerConfiguration Appceptive(
            this LoggerSinkConfiguration loggerSinkConfiguration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum, 
            IFormatProvider formatProvider = null)
        {
            if (loggerSinkConfiguration == null) 
                throw new ArgumentNullException("loggerSinkConfiguration");
            
            return loggerSinkConfiguration.Sink(new AppceptiveSink(formatProvider), restrictedToMinimumLevel);
        }
    }
}