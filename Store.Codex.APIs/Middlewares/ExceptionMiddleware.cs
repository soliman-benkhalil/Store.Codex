using Microsoft.AspNetCore.Http;
using Store.Codex.APIs.Errors;
using System.Text.Json;

namespace Store.Codex.APIs.Middlewares
{
    // every request will get throw it even the swagger request before i made a request myself
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger,IHostEnvironment env)
        {
            _next  = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context) // HttpContext -> object represents the request and response message
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.ContentType = "application/json"; // it is the format of the data that existed in the result for the user  
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = _env.IsDevelopment() ?
                    new ApiExceptionResponse(StatusCodes.Status500InternalServerError, ex.Message , ex?.StackTrace?.ToString()) // .StackTrace?.ToString() -> details
                    : new ApiExceptionResponse(StatusCodes.Status500InternalServerError);

                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var josn = JsonSerializer.Serialize(response, options); // object to byte stream (json string here)

                await context.Response.WriteAsync(josn); // here we write text that represents the body of the response  &  WriteAsync takes the data on the format of json string so we have to make serialization
            }
                
        }
    }
}
