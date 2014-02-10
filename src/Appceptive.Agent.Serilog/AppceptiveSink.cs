using System;
using Appceptive.Agent.Core;
using Serilog.Core;
using Serilog.Events;

namespace Appceptive.Agent.Serilog
{
    public class AppceptiveSink : ILogEventSink
    {                        
        readonly IFormatProvider _formatProvider;

        public AppceptiveSink(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            var level = logEvent.Level.ToString();
            var important = IsImportant(logEvent);
            var renderedMessage = logEvent.RenderMessage(_formatProvider);
            var @event = new Event(level, important, logEvent.Timestamp.UtcDateTime)
                .WithProperty("MessageTemplate", logEvent.MessageTemplate.Text)
                .WithDescription(renderedMessage);

            if (logEvent.Exception != null)
            {
                @event.WithProperty("Exception", logEvent.Exception.ToString());
            }

            foreach (var pair in logEvent.Properties)
            {
                @event.WithProperty(pair.Key, AppceptivePropertyFormatter.Simplify(pair.Value));
            }

            Core.Appceptive.AddActivityEvent(@event);
        }

        private static bool IsImportant(LogEvent logEvent)
        {
            return logEvent.Level == LogEventLevel.Error || logEvent.Level == LogEventLevel.Fatal;
        }
    }
}