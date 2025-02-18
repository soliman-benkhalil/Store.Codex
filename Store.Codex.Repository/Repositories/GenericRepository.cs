using Microsoft.EntityFrameworkCore;
using Store.Codex.Core.Entities;
using Store.Codex.Core.Repositories.Contract;
using Store.Codex.Core.Specifications;
using Store.Codex.Repository.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Codex.Repository.Repositories
{
    public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if(typeof(TEntity) == typeof(Product))
            {
                return (IEnumerable<TEntity>)await _context.Products.Include(P => P.Brand).Include(P => P.Type).ToListAsync();
            }

            return await _context.Set<TEntity>().ToListAsync(); // Set is a mothod that returns any dbset of any type
        }

        public async Task<TEntity> GetAsync(Tkey id)
        {
            if (typeof(TEntity) == typeof(Product))
            {
                //return await _context.Products.Include(P => P.Brand).Include(P => P.Type).FirstOrDefaultAsync(P=>P.Id==id as int?) as TEntity;
                return await _context.Products.Where(P => P.Id == id as int?).Include(P => P.Brand).Include(P => P.Type).FirstOrDefaultAsync() as TEntity;
            } 
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }


        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, Tkey> spec)
        {
            return await ApplySpecification(spec).ToListAsync(); // ToList Here Because the return from this like is i qurable and the function returns an i enubrable 
        }

        public async Task<TEntity> GetWithSpecAsync(ISpecification<TEntity, Tkey> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        // refactoring for better enhancment of code 
        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity, Tkey> spec)
        {
            return SpecificationsEvaluator<TEntity, Tkey>.GetQuery(_context.Set<TEntity>(), spec);
        }

        public async Task<int> GetCountAsync(ISpecification<TEntity, Tkey> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }
    }
}
