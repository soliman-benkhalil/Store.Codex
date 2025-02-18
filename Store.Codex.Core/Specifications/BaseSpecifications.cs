using Store.Codex.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Codex.Core.Specifications
{
    public class BaseSpecifications<TEntity, Tkey> : ISpecification<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public Expression<Func<TEntity, bool>> Criteria { get; set; } = null; // null  to avoid warning down
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new List<Expression<Func<TEntity, object>>>(); // here is list if it is empty so there is no include 
        public Expression<Func<TEntity, object>> OrderBy { get; set; } = null;
        public Expression<Func<TEntity, object>> OrderByDescending { get; set; } = null;
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }

        // Default Values Are null and it is not valid so we declare the values in the constructor

        public BaseSpecifications(Expression<Func<TEntity, bool>> expression) // for the where ( criterial )
        {
            Criteria = expression;
            // but here the include can not intialize it here because i have to know the type of the class in navigational property so i will make it not null by make an object 
            // and it is better to put it in the declaration above 
        }

        public BaseSpecifications()
        {
            // we made it because sometimes there is no Criteria so we need to make it null and it is by default is null and in the 2 cases we must have an object from include 
        }

        // await _context.Products.Where(P => P.Id == id as int ? ).Include(P => P.Brand).Include(P => P.Type).FirstOrDefaultAsync() as TEntity;

        public void AddOrderBy(Expression<Func<TEntity, object>> orderBy)
        {
            OrderBy = orderBy;
        }

        public void AddOrderByDescending(Expression<Func<TEntity, object>> orderbydesc)
        {
            OrderByDescending = orderbydesc;
        }

        public void ApplyPagination(int skip, int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}
