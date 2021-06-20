using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WeatherApp.Helper.Fault;
using WeatherApp.Helper.Logs;

namespace WeatherApp.Infrastructures
{
    public class FailureMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IApiLogger _logger;

        public FailureMiddleware(RequestDelegate next, IApiLogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var currentBody = context.Response.Body;

            await using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            ErrorDetail error = null;
            try
            {
                await _next(context);
            }
            catch (ApiException apiException)
            {
                _logger.LogException(apiException);
                context.Response.StatusCode = (int)apiException.StatusCode;
                error = new ErrorDetail
                {
                    ErrorCode = apiException.ErrorCode,
                    Description = apiException.Message,
                    StatusCode = apiException.StatusCode
                };
            }
            catch (Exception e)
            {
                _logger.LogException(e);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                error = new ErrorDetail
                {
                    ErrorCode = 500,
                    Description = "Internal Error"
                };
            }

            context.Response.Body = currentBody;
            memoryStream.Seek(0, SeekOrigin.Begin);

            var readToEnd = await new StreamReader(memoryStream).ReadToEndAsync();
            if (context.Response.StatusCode == 200 && context.Response.ContentType != null && !context.Response.ContentType.Contains("application/json"))
            {
                await context.Response.WriteAsync(readToEnd);
                return;
            }

            if (error != null)
            {
                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
            }
            else
            {
                await context.Response.WriteAsync(readToEnd);
            }
        }
    }
}
