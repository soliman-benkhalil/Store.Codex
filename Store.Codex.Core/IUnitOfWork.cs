using Store.Codex.Core.Entities;
using Store.Codex.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Codex.Core
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();

        // Create Repository<T> And Return 

        IGenericRepository<TEntity, Tkey> Repository<TEntity , Tkey>() where TEntity : BaseEntity<Tkey>;
    }
}
