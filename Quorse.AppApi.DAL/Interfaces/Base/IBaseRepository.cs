using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quorse.AppApi.DAL.Interfaces.Base
{
    public interface IBaseRepository<T, TK>
    {
        Task<TK> SaveAsync(T entry);
        Task<T> GetByKeyAsync(TK key);
        IQueryable<T> GetAll();
        Task<T> DeleteAsync(TK key);
    }
}
