using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _logFilePath;

        public ErrorLoggingMiddleware(RequestDelegate next, string logFilePath)
        {
            _next = next;
            _logFilePath = logFilePath;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                LogException(ex);
                throw;
            }
        }

        private void LogException(Exception ex)
        {
            // Write exception details to log file
            File.AppendAllText(_logFilePath, $"[{DateTime.Now}] {ex.ToString()} \n");
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ErrorLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorLoggingMiddleware(this IApplicationBuilder builder, string logFilePath)
        {
            return builder.UseMiddleware<ErrorLoggingMiddleware>(logFilePath);
        }
    }
}
