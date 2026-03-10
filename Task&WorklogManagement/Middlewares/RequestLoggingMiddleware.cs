using System.Diagnostics;

namespace Task_WorklogManagement.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var watch = Stopwatch.StartNew();

            var method = context.Request.Method;
            var path = context.Request.Path;

            await _next(context);

            watch.Stop();

            var status = context.Response.StatusCode;
            _logger.LogInformation("HTTP {method} {path} responded {status} in {time} ms", 
                method, path, status, watch.ElapsedMilliseconds);
        }
    }
}
