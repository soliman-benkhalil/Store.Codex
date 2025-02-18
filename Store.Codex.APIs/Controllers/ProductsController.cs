using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Codex.APIs.Attributes;
using Store.Codex.APIs.Errors;
using Store.Codex.Core.Dtos;
using Store.Codex.Core.Dtos.Products;
using Store.Codex.Core.Entities;
using Store.Codex.Core.Helper;
using Store.Codex.Core.Services.Contract;
using Store.Codex.Core.Specifications.Products;

namespace Store.Codex.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [ProducesResponseType(typeof(PaginationResponse<ProductDto>), StatusCodes.Status200OK)]
        [HttpGet]  // Get BaseUrl/api/Products  
        [Cached(100)]
        [Authorize]
        public async Task<ActionResult<PaginationResponse<ProductDto>>> GetAllProducts([FromQuery] ProductSpecParams productSpec) // endpoint
        {
            var result = await _productService.GetAllProductsAsync(productSpec);

            return Ok(result); // ok returns the Status code 200 
        }


        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("brands")]  // BaseUrl/api/Products/brands
        [Authorize]
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllBrands()
        {
            var result = await _productService.GetAllBrandsAsync();
            return Ok(result);
        }


        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("types")]  // BaseUrl/api/Products/types
        [Authorize]
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllTypes()
        {
            var result = await _productService.GetAllTypesAsync();
            return Ok(result);
        }


        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]  // BaseUrl/api/Products/id   
        [Authorize]
        public async Task<ActionResult<ProductDto>> GetProductById(int? id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(400));
            var result = await _productService.GetProductByIdAsync(id.Value);

            if (result is null) return NotFound(new ApiErrorResponse(404, $"The Product With Id: {id} Not Found At Database !"));
            return Ok(result);
        }
    }
}
