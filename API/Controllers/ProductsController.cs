using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo, IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> productTypeRepo, IMapper mapper )
        {
            _productRepo = productRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList <ProductToOutputDto>>> GetProducts()
        {
            var spec = new ProductWithBrandsAndTypesSpecification();
            var products = await _productRepo.ListAsync(spec);
            return Ok( _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToOutputDto>>(products)) ;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToOutputDto>> GetProduct(int id){
            var spec = new ProductWithBrandsAndTypesSpecification(id);
            var product = await _productRepo.GetEntityWithSpec(spec);
            if(product == null){
                return NotFound(new ApiResponse(404));
            }

            return _mapper.Map<Product, ProductToOutputDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<Product>> GetProductBrands(){
            return  Ok(await _productBrandRepo.ListAllAsync());
        }

         [HttpGet("types")]
        public async Task<ActionResult<Product>> GetProductTypes(){
            return  Ok(await _productTypeRepo.ListAllAsync());
        }

    }
}