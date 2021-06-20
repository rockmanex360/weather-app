using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using Shouldly;
using WeatherApp.Features.GetCity;
using WeatherApp.Helper.Fault;
using WeatherApp.Helper.Http;
using WeatherApp.Helper.Logs;
using Xunit;

namespace WeatherApp.Test.Features
{
    public class GetCityTest
    {
        private readonly IHttpService _httpService;
        private readonly IApiLogger _apiLogger;

        public GetCityTest()
        {
            _httpService = A.Fake<IHttpService>();
            _apiLogger = A.Fake<IApiLogger>();
        }

        [Fact]
        public async Task GetCityMustReturnSuccessAsync()
        {
            var mockCityList = new CityList
            {
                Data = new List<string>
                {
                    "FAKE City 1",
                    "FAKE City 2",
                    "FAKE City 3",
                    "FAKE City 4",
                    "FAKE City 5"
                },
                Error = false,
                Msg = "FAKE MESSAGE"
            };

            var httpResult = HttpServiceResult<CityList>.Ok(mockCityList, 200);
            var mockRequest = new Request
            {
                Country = "Indonesia"
            };

            A.CallTo(() => _httpService.Post<CityList>(A<Uri>.Ignored, A<object>.Ignored, A<Action<HttpRequestMessage>>.Ignored))
                .Returns(httpResult);

            var handler = new Handler(_apiLogger, _httpService);
            var result = await handler.Handle(mockRequest, CancellationToken.None);

            result.Cities.Count().ShouldNotBe(0);
            result.ErrorCode.ShouldBe(0);
            result.Description.ShouldBe("Success");
        }

        [Fact]
        public async Task GetCityMustThrowWhenErrorAsync()
        {
            var mockErrorCall = new ErrorCall
            {
                Cod = 500,
                Message = "FAKE Error"
            };

            var httpResult = HttpServiceResult<CityList>.Fail(mockErrorCall, 500);

            A.CallTo(() => _httpService.Post<CityList>(A<Uri>.Ignored, A<object>.Ignored, A<Action<HttpRequestMessage>>.Ignored))
                .Returns(httpResult);

            var handler = new Handler(_apiLogger, _httpService);

            await Should.ThrowAsync<ApiException>(() => handler.Handle(A.Fake<Request>(), CancellationToken.None));
        }

        [Fact]
        public async Task GetCityMustThrowWhenDataIsNullAsync()
        {
            var mockErrorCall = new ErrorCall
            {
                Cod = 500,
                Message = "FAKE Error"
            };

            HttpServiceResult<CityList> httpResult = null;

            A.CallTo(() => _httpService.Post<CityList>(A<Uri>.Ignored, A<object>.Ignored, A<Action<HttpRequestMessage>>.Ignored))
                .Returns(httpResult);

            var handler = new Handler(_apiLogger, _httpService);

            await Should.ThrowAsync<ApiException>(() => handler.Handle(A.Fake<Request>(), CancellationToken.None));
        }
    }
}
