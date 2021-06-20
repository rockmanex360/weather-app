using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using Newtonsoft.Json;
using Shouldly;
using WeatherApp.Features.GetCountry;
using WeatherApp.Helper.Fault;
using WeatherApp.Helper.Logs;
using Xunit;

namespace WeatherApp.Test.Features
{
    public class GetCountryTest
    {
        private readonly IApiLogger _apiLogger;
        private readonly IFileSystem _fileSystem;

        public GetCountryTest()
        {
            _fileSystem = A.Fake<IFileSystem>();
            _apiLogger = A.Fake<IApiLogger>();
        }

        [Fact]
        public async Task GetCountryShouldReturnSuccessAsync()
        {
            var mockJson = new List<CountryList>
            {
                new CountryList
                {
                    Name = "FAKE Country 1",
                    Code = "FK1"
                },
                new CountryList
                {
                    Name = "FAKE Country 2",
                    Code = "FK2"
                },
                new CountryList
                {
                    Name = "FAKE Country 3",
                    Code = "FK3"
                }
            };
            var mockFile = JsonConvert.SerializeObject(mockJson);

            A.CallTo(() => _fileSystem.File.ReadAllTextAsync(A<string>.Ignored, CancellationToken.None))
                .Returns(Task.FromResult(mockFile));

            var handler = new Handler(_apiLogger, _fileSystem);
            var sut = await handler.Handle(A.Fake<Request>(), CancellationToken.None);

            sut.Countries.Count().ShouldBe(mockJson.Count());
            sut.Description.ShouldBe("Success");
            sut.ErrorCode.ShouldBe(0);
        }

        [Fact]
        public async Task GetCountryShouldBeThrowWhenErrorAsync()
        {
            var mockFile = JsonConvert.SerializeObject("");

            A.CallTo(() => _fileSystem.File.ReadAllTextAsync(A<string>.Ignored, CancellationToken.None))
                .Returns(Task.FromResult(mockFile));

            var handler = new Handler(_apiLogger, _fileSystem);

            await Should.ThrowAsync<ApiException>(() => handler.Handle(A.Fake<Request>(), CancellationToken.None));
        }
    }
}
