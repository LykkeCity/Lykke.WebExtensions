using Autofac;
using Microsoft.AspNetCore.Builder;

namespace Lykke.WebExtensions
{
    public static class ApplicationBuilderExtension
    {
        public static void UseWebExtensions(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalErrorHandlerMiddleware>();
        }
    }
}
