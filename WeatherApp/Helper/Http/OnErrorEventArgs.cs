using System;
namespace WeatherApp.Helper.Http
{
    public class OnErrorEventArgs : EventArgs
    {
        public OnErrorEventArgs(Exception e)
        {
            Exception = e;
        }
        public Exception Exception { get; }
    }
}
