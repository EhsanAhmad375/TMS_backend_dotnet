using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TMS.Middlewares
{
    public class MyCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public MyCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"Incoming Request: {context.Request.Method} {context.Request.Path}");

            await _next(context);

            Console.WriteLine("Response sent to client.");
        }
    }
}