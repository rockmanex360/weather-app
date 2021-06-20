using System;
using Microsoft.Extensions.Logging;

namespace WeatherApp.Helper.Logs
{
    public interface IApiLogger
    {
        void Log<T>(string message, LogLevel level, T state);
        void LogException(Exception ex);
        void LogInformation(string message);
    }
}
