using MediatR;

namespace WeatherApp.Features.GetCurrent
{
    public class Request : IRequest<Response>
    {
        public string IpAddress { get; set; }
    }
}
