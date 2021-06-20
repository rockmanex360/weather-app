using System.Net;

namespace WeatherApp.Helper.Fault
{
    public class ErrorDetail
    {
        public int ErrorCode { get; set; }
        public string Description { get; set; }

        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;
    }

    public class ErrorCall
    {
        public string Message { get; set; }
        public int Cod { get; set; }

        public bool Error { get; set; }
        public string Msg { get; set; }
    }
}
