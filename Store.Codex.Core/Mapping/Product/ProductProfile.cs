using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Codex.Core.Dtos;
using Store.Codex.Core.Dtos.Products;
using Store.Codex.Core.Entities;
using Store.Codex.Core.Mapping.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Codex.Core.Mapping 
{
    public class ProductProfile : Profile
    {
        public ProductProfile(IConfiguration configuration) // here we did not make a field for this dependecy injection because we already use it in the constructor so no needed to use it out
        {
            CreateMap<Product, ProductDto>() // here we write the <Source , Destination>  
                .ForMember(D => D.BrandName, options => options.MapFrom(S => S.Brand.Name))
                .ForMember(D => D.TypeName, options => options.MapFrom(S => S.Type.Name))
                //.ForMember(D => D.PictureUrl, options => options.MapFrom(S => $"{configuration["BASEURL"]}/{S.PictureUrl}"));
                .ForMember(D => D.PictureUrl, options => options.MapFrom(new PictureUrlResolver(configuration)));

            CreateMap<ProductBrand, TypeBrandDto>();

            CreateMap<ProductType, TypeBrandDto>();
        }
    }
}
