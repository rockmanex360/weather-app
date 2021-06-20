using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using WeatherApp.Helper.Fault;
using WeatherApp.Helper.Http;
using WeatherApp.Helper.Logs;

namespace WeatherApp.Features.GetCity
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IApiLogger _apiLogger;
        private readonly IHttpService _httpService;
        private readonly string COUNTRY_API_URI = Environment.GetEnvironmentVariable("COUNTRY_API_URI");

        public Handler(IApiLogger apiLogger, IHttpService httpService)
        {
            _apiLogger = apiLogger;
            _httpService = httpService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            _apiLogger.LogInformation($"Get City with request data {JsonConvert.SerializeObject(request)}");

            var cities = await _httpService.Post<CityList>(new Uri($"{COUNTRY_API_URI}/countries/cities"), new
            {
                country = request.Country
            });

            if (cities == null)
                throw new ApiException(HttpStatusCode.InternalServerError, "Internal Error", 500);

            if (cities.IsFailure)
                throw new ApiException(HttpStatusCode.InternalServerError, cities.ErrorDescription, int.Parse(cities.ErrorCode));

            return new Response
            {
                Cities = cities.Value.Data
            };
        }
    }
}
