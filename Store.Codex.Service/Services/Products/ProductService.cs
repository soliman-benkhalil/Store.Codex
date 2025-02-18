using AutoMapper;
using Store.Codex.Core;
using Store.Codex.Core.Dtos;
using Store.Codex.Core.Dtos.Products;
using Store.Codex.Core.Entities;
using Store.Codex.Core.Helper;
using Store.Codex.Core.Services.Contract;
using Store.Codex.Core.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Codex.Service.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams productSpec)
        {
            var spec = new ProductSpecifications(productSpec);

            var products = await _unitOfWork.Repository<Product, int>().GetAllWithSpecAsync(spec);

            var mappedProducts = _mapper.Map<IEnumerable<ProductDto>>(products);

            var countSpec = new ProductWithCountSpecification(productSpec);

            var count = await _unitOfWork.Repository<Product, int>().GetCountAsync(countSpec);

            return new PaginationResponse<ProductDto>(productSpec.PageSize, productSpec.PageIndex, count, mappedProducts);

            //return _mapper.Map<IEnumerable<ProductDto>>(await _unitOfWork.Repository<Product, int>().GetAllWithSpecAsync(spec));
        }





        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var spec = new ProductSpecifications(id);

            return _mapper.Map<ProductDto>(await _unitOfWork.Repository<Product, int>().GetWithSpecAsync(spec));
            //return _mapper.Map<ProductDto>(await _unitOfWork.Repository<Product, int>().GetAsync(id));
            //var product =  await _unitOfWork.Repository<Product,int>().GetAsync(id);
            //var mappedProduct = _mapper.Map<ProductDto>(product); 
            //return mappedProduct;
        }

        public async Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync() => _mapper.Map<IEnumerable<TypeBrandDto>>(await _unitOfWork.Repository<ProductType, int>().GetAllAsync());


        public async Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync() => _mapper.Map<IEnumerable<TypeBrandDto>>(await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync());

    }
}
