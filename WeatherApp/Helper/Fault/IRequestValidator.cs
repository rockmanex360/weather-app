using System.Threading.Tasks;

namespace WeatherApp.Helper.Fault
{
    public interface IRequestValidator<in TRequest>
    {
        Task<ValidationResult> Validate(TRequest request);
        int Order { get; }
    }
}
