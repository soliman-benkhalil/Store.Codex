using Store.Codex.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Codex.Core.Specifications
{
    public interface ISpecification<TEntitiy,Tkey> where TEntitiy : BaseEntity<Tkey>
    {
        public Expression<Func<TEntitiy, bool>> Criteria { get; set; } // Where
        public List<Expression<Func<TEntitiy, Object>>> Includes { get; set; } // Include
        public Expression<Func<TEntitiy, object>> OrderBy { get; set; }
        public Expression<Func<TEntitiy, object>> OrderByDescending { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }

    }



}
