using BLL.IService;
using Otlob_API.ErrorModel;
using System.Net;
using System.Text.Json;

namespace Otlob_API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILoggerManager _logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger, IHostEnvironment Env)
        {
            this.next = next;
            _logger = logger;
            env = Env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the" +
                  $" {nameof(ExceptionMiddleware)} in {nameof(InvokeAsync)} method {ex}");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var responseMessage = env.IsDevelopment()
                    ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

                var serializeOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(responseMessage, serializeOptions);
                await context.Response.WriteAsync(json);
            }
        }

    }
}
