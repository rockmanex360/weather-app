using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WeatherApp.Controllers
{
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeatherController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("city")]
        public async Task<IActionResult> GetCity([FromQuery] string country)
        {
            var result = await _mediator.Send(new Features.GetCity.Request
            {
                Country = country
            });
            return Ok(result);
        }

        [HttpGet]
        [Route("country")]
        public async Task<IActionResult> GetCountry()
        {
            var result = await _mediator.Send(new Features.GetCountry.Request());
            return Ok(result);
        }

        [HttpGet]
        [Route("weather")]
        public async Task<IActionResult> GetWeather([FromQuery]string city)
        {
            var result = await _mediator.Send(new Features.GetWeather.Request
            {
                City = city,
            });
            return Ok(result);
        }

        [HttpGet]
        [Route("current")]
        public async Task<IActionResult> GetCurrent([FromQuery]string ipAddress)
        {
            var result = await _mediator.Send(new Features.GetCurrent.Request
            {
                IpAddress = ipAddress
            });
            return Ok(result);
        }
    }
}
