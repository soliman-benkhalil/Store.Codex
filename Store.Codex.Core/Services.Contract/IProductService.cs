using Store.Codex.Core.Dtos;
using Store.Codex.Core.Dtos.Products;
using Store.Codex.Core.Helper;
using Store.Codex.Core.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Codex.Core.Services.Contract
{
    public interface IProductService
    {
        Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams productSpec); 

        Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync(); 

        Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync(); 

        Task<ProductDto> GetProductByIdAsync(int id); 





    }
}
