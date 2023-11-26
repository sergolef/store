using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;

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

            CreateMap<Address, UserAddressDto>()
                .ForMember(p => p.FirstName,  s => s.MapFrom(s => s.FirstName))
                .ForMember(p => p.LastName,  s => s.MapFrom(s => s.LastName))
                .ForMember(p => p.City,  s => s.MapFrom(s => s.City))
                .ForMember(p => p.State,  s => s.MapFrom(s => s.State))
                .ForMember(p => p.ZipCode,  s => s.MapFrom(s => s.ZipCode))
                .ForMember(p => p.Street,  s => s.MapFrom(s => s.Street)).ReverseMap();
        }
    }
}