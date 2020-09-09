using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpottedUnitn.Data.DbAccess.Interfaces
{
    public interface IEntityDbAccess<T>
    {
        T Find(params object[] keyValues);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task<T> FindAsync(params object[] keyValues);
        Task AddAsync(T entity);
    }
}
