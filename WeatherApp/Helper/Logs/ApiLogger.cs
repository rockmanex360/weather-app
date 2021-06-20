using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog.Events;

namespace WeatherApp.Helper.Logs
{
    public class ApiLogger : IApiLogger
    {
        private readonly Serilog.Core.Logger _logger;
        private readonly JsonSerializerSettings _settings;
        private int _maxDeepInnerException = 1;
        public ApiLogger(Serilog.Core.Logger logger)
        {
            _logger = logger;
            _settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MaxDepth = 2,
                Error = OnSerializingError
            };
        }

        private void OnSerializingError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.CurrentObject);
            Console.WriteLine(e.ErrorContext);
        }


        public void LogException(Exception ex)
        {
            _maxDeepInnerException = 1;
            _logger.Error(GetInnerMessage(ex), ex, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        }

        private string GetInnerMessage(Exception exception)
        {
            if (_maxDeepInnerException > 5) return exception.Message;
            _maxDeepInnerException++;
            return exception.InnerException != null ? GetInnerMessage(exception.InnerException) : exception.Message;
        }

        public void Log<T>(string message, LogLevel level, T state)
        {
            switch (level)
            {
                case LogLevel.Trace:
                    _logger.Write(LogEventLevel.Verbose, message, state, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
                    break;
                case LogLevel.Debug:
                    _logger.Write(LogEventLevel.Debug, message, state, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
                    break;
                case LogLevel.Information:
                    _logger.Write(LogEventLevel.Information, message, state, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
                    break;
                case LogLevel.Warning:
                    _logger.Write(LogEventLevel.Warning, message, state, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
                    break;
                case LogLevel.Error:
                    _logger.Write(LogEventLevel.Error, message, state, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
                    break;
                case LogLevel.Critical:
                    _logger.Write(LogEventLevel.Fatal, message, state, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
                    break;
                case LogLevel.None:
                    _logger.Write(LogEventLevel.Fatal, message, state, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
                    break;
            }
        }

        public void LogInformation(string message)
        {
            _logger.Information(message, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        }
    }
}
