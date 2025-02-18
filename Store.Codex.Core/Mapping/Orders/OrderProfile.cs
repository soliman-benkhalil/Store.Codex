using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Codex.Core.Dtos.Orders;
using Store.Codex.Core.Entities.Order;

namespace Store.Codex.Core.Mapping.Orders
{
    public class OrderProfile : Profile 
    {
        public OrderProfile(IConfiguration configuration)
        {
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(D => D.DeliveryMethod, options => options.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(D => D.DeliveryMethodCost, options => options.MapFrom(S => S.DeliveryMethod.Cost));

            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(D => D.ProductId, options => options.MapFrom(S => S.Product.ProductId))
                .ForMember(D => D.ProductName, options => options.MapFrom(S => S.Product.ProductName))
                .ForMember(D => D.PictureUrl, options => options.MapFrom(S => $"{configuration["BASEURL"]}{S.Product.PictureUrl}"));

            CreateMap<DeliveryMethod, DeliveryMethodDto>()
            .ForMember(D => D.Name, opt => opt.MapFrom(S => S.ShortName))
            .ForMember(D => D.Price, opt => opt.MapFrom(S => S.Cost))
            .ReverseMap();
        }
    }
}
