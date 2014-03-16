using System.Collections;
using Appceptive.Agent.Core;
using log4net.Appender;
using log4net.Core;

namespace Appceptive.Agent.Log4Net
{

    public class AppceptiveAppender : BufferingAppenderSkeleton
    {
        
        protected override void SendBuffer(LoggingEvent[] events)
        {

            foreach (var loggingEvent in events)
            {
                var level = loggingEvent.Level.ToString();
                var important = IsImportant(loggingEvent);
                var renderedMessage = loggingEvent.RenderedMessage;
                var @event = new Event(level, important, loggingEvent.TimeStamp)
                    .WithDescription(renderedMessage);

                if (loggingEvent.ExceptionObject != null)
                {
                    @event.WithProperty("Exception", loggingEvent.ExceptionObject);
                }

                foreach (DictionaryEntry property in loggingEvent.GetProperties())
                {
                    @event.WithProperty(property.Key.ToString(), property.Value);
                }
            }            
        }

        private static bool IsImportant(LoggingEvent logEvent)
        {
            return logEvent.Level == Level.Emergency 
                || logEvent.Level == Level.Fatal
                || logEvent.Level == Level.Alert
                || logEvent.Level == Level.Critical
                || logEvent.Level == Level.Severe
                || logEvent.Level == Level.Error
                ;
        }
    }
}