using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeatherApp.Helper.Fault;

namespace WeatherApp.Infrastructures
{
    public class ValidationDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IRequestValidator<TRequest>> _validators;

        public ValidationDecorator(IEnumerable<IRequestValidator<TRequest>> validators)
        {
            _validators = validators;
        }


        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!_validators.Any()) return await next();
            foreach (var validator in _validators.OrderBy(v => v.Order))
            {
                var result = await validator.Validate(request);

                if (result.IsFailure)
                    throw new ApiException(result.HttpStatusCode, result.ErrorDescription, result.ErrorCode);
            }

            return await next();
        }
    }
}
