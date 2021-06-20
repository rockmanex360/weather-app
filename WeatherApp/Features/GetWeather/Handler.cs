using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Omu.ValueInjecter;
using WeatherApp.Helper.Fault;
using WeatherApp.Helper.Http;
using WeatherApp.Helper.Logs;

namespace WeatherApp.Features.GetWeather
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IApiLogger _apiLogger;
        private readonly IHttpService _httpService;
        private readonly string WEATHER_API_URI = Environment.GetEnvironmentVariable("WEATHER_API_URI");
        private readonly string API_KEY = Environment.GetEnvironmentVariable("WeatherApiKey");

        public Handler(IApiLogger apiLogger, IHttpService httpService)
        {
            _apiLogger = apiLogger;
            _httpService = httpService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            _apiLogger.LogInformation($"Get Weather with request data {JsonConvert.SerializeObject(request)}");

            var weather = await _httpService.Get<Response>(new Uri($"{WEATHER_API_URI}?q={request.City}&appid={API_KEY}"));

            if (weather == null)
                throw new ApiException(HttpStatusCode.InternalServerError, "Internal Error", 500);

            if (weather.IsFailure)
                throw new ApiException(HttpStatusCode.InternalServerError, weather.ErrorDescription, int.Parse(weather.ErrorCode));

            var res = new Response().InjectFrom(weather.Value);
            return (Response)res;
        }
    }
}
