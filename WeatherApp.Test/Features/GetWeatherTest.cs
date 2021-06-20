using System;
using System.Threading.Tasks;
using FakeItEasy;
using WeatherApp.Helper.Http;
using WeatherApp.Helper.Logs;
using Xunit;
using WeatherApp.Features.GetWeather;
using AutoFixture;
using System.Net.Http;
using System.Threading;
using Shouldly;
using WeatherApp.Helper.Fault;

namespace WeatherApp.Test.Features
{
    public class GetWeatherTest
    {
        private readonly IHttpService _httpService;
        private readonly IApiLogger _apiLogger;
        private readonly Fixture _fixture;

        public GetWeatherTest()
        {
            _httpService = A.Fake<IHttpService>();
            _apiLogger = A.Fake<IApiLogger>();
            _fixture = new Fixture();

            Environment.SetEnvironmentVariable("WEATHER_API_URI", "https://fake.com");
            Environment.SetEnvironmentVariable("WeatherApiKey", "FAKE");
        }

        [Fact]
        public async Task GetWeatherShouldReturnSuccessAsync()
        {
            var request = new Request
            {
                City = "Bandung"
            };

            var response = _fixture.Build<Response>()
                .With(x => x.Name, request.City)
                .Create();

            var httpServiceResult = HttpServiceResult<Response>.Ok(response, 200);

            A.CallTo(() => _httpService.Get<Response>(A<Uri>.Ignored, A<Action<HttpRequestMessage>>.Ignored))
                .Returns(httpServiceResult);

            var handler = new Handler(_apiLogger, _httpService);
            var sut = await handler.Handle(request, CancellationToken.None);

            sut.ShouldNotBe(null);
            sut.Name.ShouldBe(request.City);
        }

        [Fact]
        public async Task GetWeatherShouldReturnErrorWhenHttpServiceIsNullAsync()
        {
            HttpServiceResult<Response> httpServiceResult = null;

            A.CallTo(() => _httpService.Get<Response>(A<Uri>.Ignored, A<Action<HttpRequestMessage>>.Ignored))
                .Returns(httpServiceResult);

            var handler = new Handler(_apiLogger, _httpService);

            await Should.ThrowAsync<ApiException>(() => handler.Handle(A.Fake<Request>(), CancellationToken.None));
        }

        [Fact]
        public async Task GetWeatherShouldReturnErrorWhenHttpServiceIsFailureAsync()
        {
            var errorCall = new ErrorCall
            {
                Cod = 500,
                Message = "FAKE Error"
            };

            var httpServiceResult = HttpServiceResult<Response>.Fail(errorCall, 500);

            A.CallTo(() => _httpService.Get<Response>(A<Uri>.Ignored, A<Action<HttpRequestMessage>>.Ignored))
                .Returns(httpServiceResult);

            var handler = new Handler(_apiLogger, _httpService);

            await Should.ThrowAsync<ApiException>(() => handler.Handle(A.Fake<Request>(), CancellationToken.None));
        }
    }
}
