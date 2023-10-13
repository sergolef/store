using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;

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
        }
    }
}