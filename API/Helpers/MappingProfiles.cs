using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToOutputDto>()
                .ForMember(p => p.ProductBrand, s => s.MapFrom( s => s.ProductBrand.Name))
                .ForMember(p => p.ProductType, s => s.MapFrom(s => s.ProductType.Name))
                .ForMember(p => p.PictureUrl, s => s.MapFrom<ProductUrlResolver>());

            CreateMap<Core.Entities.Identity.Address, UserAddressDto>()
                .ForMember(p => p.FirstName,  s => s.MapFrom(s => s.FirstName))
                .ForMember(p => p.LastName,  s => s.MapFrom(s => s.LastName))
                .ForMember(p => p.City,  s => s.MapFrom(s => s.City))
                .ForMember(p => p.State,  s => s.MapFrom(s => s.State))
                .ForMember(p => p.ZipCode,  s => s.MapFrom(s => s.ZipCode))
                .ForMember(p => p.Street,  s => s.MapFrom(s => s.Street)).ReverseMap();
            
            CreateMap<CustomerBasket, CustomerBusketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
            CreateMap<UserAddressDto, Core.Entities.OrderAggregate.Address>();
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(o => o.ProductId, s => s.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(o => o.ProductName, s => s.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(o => o.PictureUrl, s => s.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(o => o.PictureUrl, s => s.MapFrom<OrderItemUrlResolver>());

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(p => p.DeliveryMethod, s => s.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(p => p.ShippingPrice, s => s.MapFrom(s => s.DeliveryMethod.Price));

        }
    }
}