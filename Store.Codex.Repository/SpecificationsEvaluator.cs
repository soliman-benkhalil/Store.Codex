using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Store.Codex.Core.Entities;
using Store.Codex.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Codex.Repository
{
    // this class to generate the query through the function in it 
    public class SpecificationsEvaluator<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        // Create And Return Qeury
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity, Tkey> spec) // inputQuery here is like _context.Products
        {
            var query = inputQuery;

            if (spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending is not null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPaginationEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            // P => P.Brand
            // P => P.Type

            // _context.Products.Include( P => P.Brand )
            // _context.Products.Include( P => P.Brand ).Include( P => P.Type )

            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression)); // it concatenates the includes in the Includes Attribute with the query string



            return query;
        }

        // await _context.Products.Where(P => P.Id == id as int ? ).Include(P => P.Brand).Include(P => P.Type).FirstOrDefaultAsync() as TEntity;
    }
}
