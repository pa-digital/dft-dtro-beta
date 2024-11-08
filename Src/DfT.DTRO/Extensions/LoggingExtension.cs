using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DfT.DTRO.Extensions
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LogType
    {
        DEBUG,
        INFO,
        WARN,
        ERROR
    }

    public class LoggingExtension
    {
        public LogType Type { get; private set; }
        public string CalledFromMethod { get; private set; }
        public string Endpoint { get; private set; }
        public string AppId { get; private set; }
        public string Message { get; private set; }
        public string ExceptionMessage { get; private set; }

        public class Builder
        {
            private readonly LoggingExtension _loggingExtension;

            public Builder()
            {
                _loggingExtension = new LoggingExtension();
            }

            public Builder WithLogType(LogType type)
            {
                _loggingExtension.Type = type;
                return this;
            }

            public Builder WithMethodCalledFrom(string method)
            {
                _loggingExtension.CalledFromMethod = method;
                return this;
            }

            public Builder WithEndpoint(string endpoint)
            {
                _loggingExtension.Endpoint = endpoint;
                return this;
            }

            public Builder WithAppId(string appId)
            {
                _loggingExtension.AppId = appId;
                return this;
            }

            public Builder WithMessage(string message)
            {
                _loggingExtension.Message = message;
                return this;
            }

            public Builder WithExceptionMessage(string exceptionMessage)
            {
                _loggingExtension.ExceptionMessage = exceptionMessage;
                return this;
            }

            public LoggingExtension Build()
            {
                return _loggingExtension;
            }
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        public void PrintToConsole()
        {
            Console.WriteLine(ToString());
        }

        public void LogInformation(string methodName, string endpoint, string message)
        {
            new Builder()
                .WithLogType(LogType.INFO)
                .WithMethodCalledFrom(methodName)
                .WithEndpoint(endpoint)
                .WithMessage(message)
                .Build()
                .PrintToConsole();
        }

        public void LogError(string methodName, string endpoint, string message, string exceptionMessage)
        {
            new Builder()
                .WithLogType(LogType.ERROR)
                .WithMethodCalledFrom(methodName)
                .WithEndpoint(endpoint)
                .WithMessage(message)
                .WithExceptionMessage(exceptionMessage)
                .Build()
                .PrintToConsole();
        }

        public void LogWarn(string methodName, string message)
        {
            new Builder()
                .WithLogType(LogType.WARN)
                .WithMethodCalledFrom(methodName)
                .WithMessage(message)
                .Build()
                .PrintToConsole();
        }
    }
}

