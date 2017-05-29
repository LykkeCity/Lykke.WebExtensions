using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Lykke.WebExtensions
{
    public static class HttpContextExtensions
    {
        public static Uri GetUri(this HttpRequest request)
        {
            var hostComponents = request.Host.ToUriComponent().Split(':');

            var builder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = hostComponents[0],
                Path = request.Path,
                Query = request.QueryString.ToUriComponent()
            };

            if (hostComponents.Length == 2)
            {
                builder.Port = Convert.ToInt32(hostComponents[1]);
            }

            return builder.Uri;
        }

        public static string GetUserAgent(this HttpRequest request, string name)
        {
            var parametersDict = request.GetUserAgent();
            return parametersDict.ContainsKey(name) ? parametersDict[name] : null;
        }

        public static IDictionary<string, string> GetUserAgent(this HttpRequest request)
        {
            var userAgent = request.Headers["User-Agent"].ToString();

            var parameters = userAgent.Split(';');
            return parameters.Select(parameter => parameter.Split('=')).Where(kv => kv.Length > 1).ToDictionary(kv => kv[0], kv => kv[1]);
        }
    }
}
