using SpottedUnitn.Infrastructure.Services;
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

        public EntityDbAccess(ModelContext modelContext, IDateTimeOffsetService dtoService)
        {
            this.modelContext = modelContext;
            this.dtoService = dtoService;
        }
    }
}
