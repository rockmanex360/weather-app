using System.Collections.Generic;
using WeatherApp.Helper;

namespace WeatherApp.Features.GetCity
{
    public class Response : BaseResponse
    {
        public IEnumerable<string> Cities { get; set; }
    }

    public class CityList
    {
        public bool Error { get; set; }
        public string Msg { get; set; }
        public IEnumerable<string> Data { get; set; }
    }
}
