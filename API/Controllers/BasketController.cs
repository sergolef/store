using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            this._basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetCustomerBasketAsync(id);

            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>>UpdateBasket(CustomerBusketDto basket)
        {
            var customerBasket = _mapper.Map<CustomerBusketDto, CustomerBasket>(basket);
            var updated = await _basketRepository.UpdateCustomerBusket(customerBasket);

            return Ok(updated);
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteCustomerBasket(id);
        }
    }
}