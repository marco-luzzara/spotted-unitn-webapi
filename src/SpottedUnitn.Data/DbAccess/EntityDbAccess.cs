using SpottedUnitn.Infrastructure.Services;
using SpottedUnitn.Infrastructure.Services.FileStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpottedUnitn.Data.DbAccess
{
    public abstract class EntityDbAccess<T>
        where T : class
    {
        protected ModelContext modelContext;
        protected IDateTimeOffsetService dtoService;
        protected IFileStorageService fileStorageService;

        public EntityDbAccess(ModelContext modelContext, IDateTimeOffsetService dtoService, IFileStorageService fileStorageService)
        {
            this.modelContext = modelContext;
            this.dtoService = dtoService;
            this.fileStorageService = fileStorageService;
        }
    }
}
