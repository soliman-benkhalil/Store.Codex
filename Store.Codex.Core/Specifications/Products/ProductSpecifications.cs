using Store.Codex.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Codex.Core.Specifications.Products
{
    public class ProductSpecifications : BaseSpecifications<Product,int>
    {

        public ProductSpecifications(int id):base(P => P.Id == id) // it is the where (Criteria)  
        {
            ApplyIncludes();
        }


        // 900
        // P.Z = 50
        // P.I = 2
        public ProductSpecifications(ProductSpecParams productSpec) : base // Here brandId and typeId is used in where so Criteria
            (
                P =>
                (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search))
                &&
                (!productSpec.BrandId.HasValue || productSpec.BrandId == P.BrandId) 
                && 
                (!productSpec.TypeId.HasValue || productSpec.TypeId == P.TypeId)
            )
        {
            // name, priceAsc ,PriceDesc 

            if (!string.IsNullOrEmpty(productSpec.Sort))
            {
                switch (productSpec.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price); break;
                    case "PriceDesc":
                        AddOrderByDescending(P => P.Price); break;
                    default: OrderBy = P => P.Name; break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }

            ApplyIncludes();

            ApplyPagination(productSpec.PageSize* (productSpec.PageIndex - 1), productSpec.PageSize);
        }

        private void ApplyIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Type);
        }
    }
}
