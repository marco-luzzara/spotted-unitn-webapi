using SpottedUnitn.Data.DbAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpottedUnitn.Data.DbAccess
{
    public abstract class EntityDbAccess<T> : IEntityDbAccess<T>
        where T : class
    {
        protected ModelContext modelContext;

        public EntityDbAccess(ModelContext modelContext)
        {
            this.modelContext = modelContext;
        }

        public void Add(T entity)
        {
            this.modelContext.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await this.modelContext.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            this.modelContext.Remove(entity);
        }

        public T Find(params object[] keyValues)
        {
            return this.modelContext.Find<T>(keyValues);
        }

        public async Task<T> FindAsync(params object[] keyValues)
        {
            return await this.modelContext.FindAsync<T>(keyValues);
        }

        public void Update(T entity)
        {
            this.modelContext.Update(entity);
        }
    }
}
