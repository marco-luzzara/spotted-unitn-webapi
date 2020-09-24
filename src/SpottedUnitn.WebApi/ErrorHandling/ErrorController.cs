using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SpottedUnitn.WebApi.Controllers
{
    [Route("error")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        protected ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        [Route("production")]
        public IActionResult ErrorOnProduction([FromServices] IWebHostEnvironment webHostEnvironment)
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exc = context.Error;
            var errorId = Guid.NewGuid();

            this.logger.LogError(exc, $"exception with Id={errorId} for user {HttpContext.User.Identity.Name ?? "Anonymous"}");

            return Problem(
                detail: errorId.ToString(),
                title: context.Error.Message);
        }
    }
}
