using System.Collections.Generic;
using WeatherApp.Helper;

namespace WeatherApp.Features.GetCountry
{
    public class Response : BaseResponse
    {
        public IEnumerable<string> Countries { get; set; }
    }

    public class CountryList
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
