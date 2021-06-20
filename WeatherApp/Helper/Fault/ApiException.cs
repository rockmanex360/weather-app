using System;
using System.Net;

namespace WeatherApp.Helper.Fault
{
    public class ApiException : Exception
    {
        public const string ErrorMessage = "Something went wrong. Please try again in a few minutes or contact your support team";

        public HttpStatusCode StatusCode { get; }

        public ApiException(HttpStatusCode status, string message, int resultErrorCode) : base(message)
        {
            StatusCode = status;
            ErrorCode = resultErrorCode;
        }

        public ApiException(HttpStatusCode status, string message, int resultErrorCode, Exception innerException) : base(message, innerException)
        {
            StatusCode = status;
            ErrorCode = resultErrorCode;

        }

        public int ErrorCode { get; set; }

        public string EventId { get; set; }

        public static ApiException InternalServerError => new ApiException(HttpStatusCode.BadRequest, ErrorMessage, 500);
    }
}
