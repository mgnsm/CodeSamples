using Microsoft.Extensions.Logging;
using System;

namespace AvroTypes
{
    public static class Extensions
    {
        public static v1.EventId GetEventId(this EventId eventId)
        {
            if (eventId == null)
                return default;

            return new AvroTypes.v1.EventId() { Id = eventId.Id, Name = eventId.Name };
        }

        public static v1.Exception GetException(this Exception exception)
        {
            if (exception == null)
                return default;

            return new v1.Exception()
            {
                StackTrace = exception.StackTrace,
                Source = exception.Source,
                Message = exception.Message,
                HResult = exception.HResult,
                HelpLink = exception.HelpLink
            };
        }

        public static v1.LogLevel GetLogLevel(this LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return v1.LogLevel.Trace;
                case LogLevel.Debug:
                    return v1.LogLevel.Debug;
                case LogLevel.Information:
                    return v1.LogLevel.Information;
                case LogLevel.Warning:
                    return v1.LogLevel.Warning;
                case LogLevel.Error:
                    return v1.LogLevel.Error;
                case LogLevel.Critical:
                    return v1.LogLevel.Critical;
                default:
                    throw new ArgumentException($"Unknown {nameof(LogLevel)}: {logLevel}", nameof(logLevel));
            }
        }
    }
}