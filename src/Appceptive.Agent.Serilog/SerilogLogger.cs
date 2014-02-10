using System;
using Serilog;

namespace Appceptive.Agent.Serilog
{
    public class SerilogLogger : Core.Logging.ILogger
    {
        private readonly ILogger _logger;

        public SerilogLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Debug(string format, params object[] args)
        {
            _logger.Debug(format, args);
        }

        public void Information(string format, params object[] args)
        {
            _logger.Information(format, args);
        }

        public void Warning(string format, params object[] args)
        {
            _logger.Warning(format, args);
        }

        public void Error(string format, params object[] args)
        {
            _logger.Error(format, args);
        }

        public void Error(Exception ex, string format, params object[] args)
        {
            _logger.Error(ex, format, args);
        }
    }
}