using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Microsoft.AspNetCore.Http;

namespace Lykke.WebExtensions
{
    public class GlobalErrorHandlerMiddleware : IMiddleware
    {
        private readonly ILog _log;
        private readonly string _component;

        public GlobalErrorHandlerMiddleware(ILog log, string component)
        {
            _log = log;
            _component = component;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                await LogError(context, ex);

                var encoding = Encoding.UTF8;
                var messageBytes = encoding.GetBytes(ex.Message);

                context.Response.StatusCode = 500;
                context.Response.ContentType = $"text/plain; charset={encoding.WebName}";
                context.Response.ContentLength = messageBytes.Length;
                await context.Response.Body.WriteAsync(messageBytes, 0, messageBytes.Length);
            }
        }

        private async Task LogError(HttpContext context, Exception ex)
        {
            await _log.WriteErrorAsync(_component, nameof(GlobalErrorHandlerMiddleware), context.Request.GetUri().AbsoluteUri, ex);
        }
    }
}