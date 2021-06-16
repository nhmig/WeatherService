using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WeatherService.Middleware
{
    public static class LogURLMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogUrl(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LogURLMiddleware>();
        }
    }


    public class LogURLMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogURLMiddleware> _logger;
        public LogURLMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory?.CreateLogger<LogURLMiddleware>() ??
            throw new ArgumentNullException(nameof(loggerFactory));
}
        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"Request URL: {UriHelper.GetDisplayUrl(context.Request)}");
            await this._next(context);
        }
    }
}
