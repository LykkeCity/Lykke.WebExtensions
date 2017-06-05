using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.WebExtensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddWebExtensions(this IMvcBuilder mvc)
        {
            mvc.AddApplicationPart(typeof(MvcBuilderExtensions).GetTypeInfo().Assembly).AddControllersAsServices();
            return mvc;
        }
    }
}
