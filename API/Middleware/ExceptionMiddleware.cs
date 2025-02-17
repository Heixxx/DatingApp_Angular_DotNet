using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

public class ExceptionMiddleware{

    readonly RequestDelegate _next;
    readonly ILogger<ExceptionMiddleware> _logger;
    readonly IHostEnvironment _env;
//                              Wyjątek             Wyswietlenie bledu w konsoli          W jakim trybie działamy (np. dev)
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env){
        _next = next;
        _logger = logger;
        _env = env;
    }
    public async Task InvokeAsync(HttpContext context){    //InvokeAsync - jest domyślnie w middleware.
        try{
            await _next(context);
        }
        catch(System.Exception ex){
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = _env.IsDevelopment() ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
            : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");

            var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

            var json = JsonSerializer.Serialize(response,options);
            await context.Response.WriteAsync(json);
        }
    }
} 