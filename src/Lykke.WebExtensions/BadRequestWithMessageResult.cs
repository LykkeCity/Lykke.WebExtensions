using Microsoft.AspNetCore.Mvc;

namespace Lykke.WebExtensions
{
    internal class BadRequestWithMessageResult : ContentResult
    {
        public BadRequestWithMessageResult(string message)
        {
            StatusCode = 400;
            ContentType = "text/plain";
            Content = message;
        }
    }
}