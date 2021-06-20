using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using WeatherApp.Helper.Fault;
using WeatherApp.Helper.Logs;

namespace WeatherApp.Features.GetCurrent
{
    public class Validation : AbstractValidator<Request>, IRequestValidator<Request>
    {
        private readonly IApiLogger _apiLogger;

        public Validation(IApiLogger apiLogger)
        {
            _apiLogger = apiLogger;

            RuleFor(x => x.IpAddress)
                .NotNull()
                .NotEmpty()
                .WithMessage("IP Address cannot be empty !")
                .Must(x => ValidateIP(x))
                .WithMessage("IP Address is invalid !");
        }

        private bool ValidateIP(string x) => IPAddress.TryParse(x, out _);

        public int Order => 1;

        public new async Task<ValidationResult> Validate(Request request)
        {
            var result = await ValidateAsync(request);
            if (result.IsValid) return ValidationResult.Ok();
            _apiLogger.Log(
                $"Following fields are mandatory: {string.Join(", ", result.Errors.Select(x => x.ErrorMessage))}",
                LogLevel.Warning, result.Errors);

            throw new ApiException(HttpStatusCode.BadRequest ,$"Invalid request. {string.Join(",", result.Errors.Select(x => x.ErrorMessage))}", 2);
        }
    }
}
