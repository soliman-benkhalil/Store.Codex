using Store.Codex.Core;
using Store.Codex.Core.Entities;
using Store.Codex.Core.Repositories.Contract;
using Store.Codex.Repository.Data.Contexts;
using Store.Codex.Repository.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Codex.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _context;

        private Hashtable _repositories; // It Is for and checks is the repository is existed or not

        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        



        public IGenericRepository<TEntity, Tkey> Repository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity, Tkey>(_context); // here we put (_context) because the construcotr of the GenericRepository takes a parameter of context 
                _repositories.Add(type, repository);
            }

            return _repositories[type] as IGenericRepository<TEntity, Tkey>;

        }
    }
}
