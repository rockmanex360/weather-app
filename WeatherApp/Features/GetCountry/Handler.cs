using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using WeatherApp.Helper.Fault;
using WeatherApp.Helper.Logs;

namespace WeatherApp.Features.GetCountry
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IApiLogger _apiLogger;
        private readonly IFileSystem _fileSystem;

        public Handler(IApiLogger apiLogger, IFileSystem fileSystem)
        {
            _apiLogger = apiLogger;
            _fileSystem = fileSystem;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            _apiLogger.LogInformation($"Get Country with request data {JsonConvert.SerializeObject(request)}");

            var countriesFile = await _fileSystem.File.ReadAllTextAsync(@"Data/Countries.json", cancellationToken);

            if (countriesFile.Length < 3)
                throw new ApiException(HttpStatusCode.InternalServerError, "Country list not found !", 12);

            var countriesJson = JsonConvert.DeserializeObject<List<CountryList>>(countriesFile);

            return new Response
            {
                Countries = countriesJson.Select(x => x.Name).ToList()
            };
        }
    }
}
