using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherApp.Helper.Fault;

namespace WeatherApp.Helper.Http
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _client;
        public event EventHandler<AfterResponseEventArgs> AfterResponseEventHandler;
        public event EventHandler<OnErrorEventArgs> OnErrorEventHandler;

        public HttpService()
        {
            _client = new HttpClient();
        }


        public virtual async Task<HttpServiceResult<T>> Get<T>(Uri uri, Action<HttpRequestMessage> action = null) where T : class
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                action?.Invoke(request);

                var response = await _client.SendAsync(request);
                OnAfterResponseEventHandler(new AfterResponseEventArgs
                {
                    Response = response
                });
                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var g = JsonConvert.DeserializeObject<T>(result);
                    return HttpServiceResult<T>.Ok(g, (int)response.StatusCode);
                }

                var failedJson = JsonConvert.DeserializeObject<ErrorCall>(result);

                return failedJson != null
                    ? HttpServiceResult<T>.Fail(
                        string.IsNullOrEmpty(failedJson.Message) ? failedJson.Msg : failedJson.Message,
                        failedJson.Cod == 0 ? "505" : failedJson.Cod.ToString(),
                        (int)response.StatusCode)
                    : HttpServiceResult<T>.Fail($"Error occurred while performing post to {uri}: {response} - {result}", null, (int)response.StatusCode);
            }
            catch (Exception e)
            {
                ErrorEventHandler(new OnErrorEventArgs(e));
                throw;
            }
        }

        public virtual async Task<HttpServiceResult<T>> Post<T>(Uri uri, object resource, Action<HttpRequestMessage> action = null) where T : class
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, uri);

                action?.Invoke(request);

                request.Content = new StringContent(JsonConvert.SerializeObject(resource), Encoding.UTF8, "application/json");
                var response = await _client.SendAsync(request);
                OnAfterResponseEventHandler(new AfterResponseEventArgs
                {
                    Response = response
                });
                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return HttpServiceResult<T>.Ok(JsonConvert.DeserializeObject<T>(result), (int)response.StatusCode);
                }

                var failedJson = JsonConvert.DeserializeObject<ErrorCall>(result);

                return failedJson != null
                    ? HttpServiceResult<T>.Fail(
                        string.IsNullOrEmpty(failedJson.Message) ? failedJson.Msg : failedJson.Message,
                        failedJson.Cod == 0 ? "505" : failedJson.Cod.ToString(),
                        (int)response.StatusCode)
                    : HttpServiceResult<T>.Fail($"Error occurred while performing post to {uri}: {response} - {result}", null, (int)response.StatusCode);
            }
            catch (Exception e)
            {
                ErrorEventHandler(new OnErrorEventArgs(e));
                throw;
            }
        }

        public void OnAfterResponseEventHandler(AfterResponseEventArgs e)
        {
            var handler = AfterResponseEventHandler;
            handler?.Invoke(this, e);
        }

        protected void ErrorEventHandler(OnErrorEventArgs e)
        {
            var handler = OnErrorEventHandler;
            handler?.Invoke(this, e);
        }
    }
}
