using Exchange.Items.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Exchange.Infrastructure.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                await HandleErrorAsync(context, exception);
            }
        }

        private static Task HandleErrorAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            object response;

            if (exception is BusinessException applicationException)
            {
                context.Response.StatusCode = applicationException.StatusCode;

                response = new
                {
                    message = exception.Message
                };
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                response = new
                {
                    message = exception.Message,
                    exception = exception.ToString()
                };
            }

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
