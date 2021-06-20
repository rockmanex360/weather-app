using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherApp.Helper.Http
{
    public interface IHttpService
    {
        Task<HttpServiceResult<T>> Get<T>(Uri uri, Action<HttpRequestMessage> action = null) where T : class;

        Task<HttpServiceResult<T>> Post<T>(Uri uri, object resource, Action<HttpRequestMessage> action = null) where T : class;
    }
}
