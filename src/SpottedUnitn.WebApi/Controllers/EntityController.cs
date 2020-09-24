using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SpottedUnitn.WebApi.ErrorHandling;

namespace SpottedUnitn.WebApi.Controllers
{
    [ApiController]
    public class EntityController : ControllerBase
    {
        protected ICustomExceptionHandler excHandler;
        public EntityController(ICustomExceptionHandler excHandler)
        {
            this.excHandler = excHandler;
        }

        private object HandleIfCustomTypeException(object value)
        {
            try
            {
                if (value is Exception excValue)
                    return this.excHandler.HandleException(excValue);
                else
                    return value;
            }
            catch (InvalidOperationException)
            {
                return value;
            }
        }

        public override NotFoundObjectResult NotFound([ActionResultObjectValue] object value)
        {
            return base.NotFound(HandleIfCustomTypeException(value));
        }

        public override ConflictObjectResult Conflict([ActionResultObjectValue] object error)
        {
            return base.Conflict(HandleIfCustomTypeException(error));
        }

        public override BadRequestObjectResult BadRequest([ActionResultObjectValue] object error)
        {
            return base.BadRequest(HandleIfCustomTypeException(error));
        }
    }
}
