using MediatR;

namespace WeatherApp.Features.GetCity
{
    public class Request : IRequest<Response>
    {
        public string Country { get; set; }
    }
}
