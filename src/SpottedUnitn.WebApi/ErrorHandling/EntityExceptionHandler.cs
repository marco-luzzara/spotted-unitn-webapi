using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SpottedUnitn.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpottedUnitn.WebApi.ErrorHandling
{
    public class EntityExceptionHandler : ICustomExceptionHandler
    {
        protected delegate EntityExceptionData FormatExceptionOnEnvDelegate(EntityException exc);
        protected readonly FormatExceptionOnEnvDelegate FormatExceptionOnEnv;
        protected readonly ILogger<EntityExceptionHandler> logger;
        protected readonly IStringLocalizer<EntityExceptionHandler> localizer;

        public EntityExceptionHandler(IWebHostEnvironment env, 
            ILogger<EntityExceptionHandler> logger, 
            IStringLocalizer<EntityExceptionHandler> localizer)
        {
            this.logger = logger;
            this.localizer = localizer;

            if (env.IsDevelopment())
                FormatExceptionOnEnv = HandleEntityExceptionForDevelopment;
            else
                FormatExceptionOnEnv = HandleEntityExceptionDefault;
        }

        public object HandleException(Exception exc)
        {
            if (exc is EntityException entityExc)
                return HandleEntityException(entityExc);
            else
                throw new InvalidOperationException($"handler {nameof(EntityExceptionHandler)} can only handle EntityExceptions");
        }

        protected string GetKeyForLocalizedException(EntityException exc)
        {
            return $"{exc.GetType().Name}.{exc.GetCodeName()}";
        }

        protected string GetLocalizedExceptionMessage(EntityException exc)
        {
            var localizedExcKey = GetKeyForLocalizedException(exc);
            var formattedMessage = this.localizer[localizedExcKey];
            return string.Format(formattedMessage, exc.MessageParams);
        }

        protected EntityExceptionData HandleEntityException<TEntityException>(TEntityException exc)
            where TEntityException : EntityException
        {
            this.logger.LogError(exc, exc.Message);
            return FormatExceptionOnEnv(exc);
        }


        protected EntityExceptionData HandleEntityExceptionForDevelopment<TEntityException>(TEntityException exc)
            where TEntityException : EntityException
        {
            return new EntityExceptionDataOnDevelopment()
            {
                Code = exc.Code,
                Message = GetLocalizedExceptionMessage(exc),
                StackTrace = exc.StackTrace
            };
        }

        protected EntityExceptionData HandleEntityExceptionDefault<TEntityException>(TEntityException exc)
            where TEntityException : EntityException
        {
            return new EntityExceptionData()
            {
                Code = exc.Code,
                Message = GetLocalizedExceptionMessage(exc),
            };
        }

        public class EntityExceptionData
        {
            public int Code { get; set; }

            public string Message { get; set; }
        }

        public class EntityExceptionDataOnDevelopment : EntityExceptionData
        {
            public string StackTrace { get; set; }
        }
    }
}
