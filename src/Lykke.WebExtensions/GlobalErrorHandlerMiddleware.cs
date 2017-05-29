using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Microsoft.AspNetCore.Http;

namespace Lykke.WebExtensions
{
    public class GlobalErrorHandlerMiddleware
    {
        private readonly ILog _log;
        private readonly RequestDelegate _next;

        public GlobalErrorHandlerMiddleware(RequestDelegate next, ILog log)
        {
            _log = log;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await LogError(context, ex);
            }
        }

        private async Task LogError(HttpContext context, Exception ex)
        {
            using (var ms = new MemoryStream())
            {
                context.Request.Body.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
                await _log.LogPartFromStream(ms, "GlobalHandler", context.Request.GetUri().AbsoluteUri, ex);
            }

            var encoding = Encoding.UTF8;
            var messageBytes = encoding.GetBytes(ex.Message);

            context.Response.StatusCode = 500;
            context.Response.ContentType = $"text/plain; charset={encoding.WebName}";
            context.Response.ContentLength = messageBytes.Length;
            await context.Response.Body.WriteAsync(messageBytes, 0, messageBytes.Length);
        }
    }
}