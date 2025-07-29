using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SkillHubApi.Middleware
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
            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation($"Incoming request: {context.Request.Method} {context.Request.Path}");

            await _next(context);

            stopwatch.Stop();
            _logger.LogInformation($"Request completed: {context.Request.Method} {context.Request.Path} | Status: {context.Response.StatusCode} | Time: {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}