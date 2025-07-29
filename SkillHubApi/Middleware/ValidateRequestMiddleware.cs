using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SkillHubApi.Middleware
{
    public class ValidateRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidateRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Ensure JSON for POST/PUT
            if (context.Request.Method == "POST" || context.Request.Method == "PUT")
            {
                if (!context.Request.HasJsonContentType())
                {
                    context.Response.StatusCode = 415;
                    await context.Response.WriteAsync("Content-Type must be application/json");
                    return;
                }
            }

            await _next(context);
        }
    }
}