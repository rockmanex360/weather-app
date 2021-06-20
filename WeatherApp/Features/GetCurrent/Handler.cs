using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace WeatherApp.Features.GetCurrent
{
    public class Handler : IRequestHandler<Request, Response>
    {
        public Handler()
        {
        }

        public Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
