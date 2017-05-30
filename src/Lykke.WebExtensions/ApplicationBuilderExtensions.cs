using Microsoft.AspNetCore.Builder;

namespace Lykke.WebExtensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseWebExtensions(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalErrorHandlerMiddleware>();
        }
    }
}
