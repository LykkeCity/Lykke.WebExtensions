using System;
using Lykke.WebExtensions.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Lykke.WebExtensions.Controllers
{
    [Route("api/[controller]")]
    public class IsAliveController : Controller
    {
        [HttpGet]
        public ActionResult Get()
        {
            return new JsonResult(new IsAliveResponse
            {
                Version =
                    Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationVersion,
                Env = Environment.GetEnvironmentVariable("ENV_INFO"),
            }, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            });
        }
    }
}
