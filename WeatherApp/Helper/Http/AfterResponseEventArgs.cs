using System;
using System.Net.Http;

namespace WeatherApp.Helper.Http
{
    public class AfterResponseEventArgs : EventArgs
    {
        public HttpResponseMessage Response { get; internal set; }
    }
}
