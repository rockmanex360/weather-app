using System.Net;

namespace WeatherApp.Helper.Fault
{
    public class ValidationResult
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;

        public string ErrorDescription { get; set; }
        public int ErrorCode { get; set; }

        public string EventId { get; set; }
        public HttpStatusCode HttpStatusCode { get; }

        private ValidationResult(bool isSuccessful, HttpStatusCode httpStatusCode, string errorDescription, int errorCode = 0)
        {
            IsSuccessful = isSuccessful;
            ErrorCode = errorCode;
            ErrorDescription = errorDescription;
            HttpStatusCode = httpStatusCode;
        }

        public static ValidationResult Ok()
        {
            return new ValidationResult(true, HttpStatusCode.OK, null);
        }

        public static ValidationResult BadRequest(string errorDescription, int errorCode = (int)HttpStatusCode.BadRequest)
        {
            return new ValidationResult(false, HttpStatusCode.BadRequest, errorDescription, errorCode);
        }

        public static ValidationResult NotFound(string errorDescription, int errorCode = (int)HttpStatusCode.NotFound)
        {
            return new ValidationResult(false, HttpStatusCode.NotFound, errorDescription, errorCode);
        }

        public static ValidationResult InternalServerError(string errorDescription, int errorCode = (int)HttpStatusCode.InternalServerError)
        {
            return new ValidationResult(false, HttpStatusCode.InternalServerError, errorDescription, errorCode);
        }

        public static ValidationResult ValidationError(string errorDescription, int errorCode = (int)HttpStatusCode.BadRequest)
        {
            return new ValidationResult(false, HttpStatusCode.BadRequest, errorDescription, errorCode);
        }

    }
}
