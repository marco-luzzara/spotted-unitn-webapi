using SpottedUnitn.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SpottedUnitn.WebApi.ErrorHandling.EntityExceptionHandler;

namespace SpottedUnitn.WebApi.ErrorHandling
{
    public interface ICustomExceptionHandler
    {
        object HandleException(Exception exc);
    }
}
