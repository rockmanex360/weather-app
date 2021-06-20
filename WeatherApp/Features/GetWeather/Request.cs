using MediatR;

namespace WeatherApp.Features.GetWeather
{
    public class Request : IRequest<Response>
    {
        public string City { get; set; }
    }
}
