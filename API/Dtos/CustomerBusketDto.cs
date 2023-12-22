using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class CustomerBusketDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();

        public int? DeliveryMethodId  { get; set; }
        
        public string ClientSecret { get; set; }

        public string PaymentIntentId { get; set; }

        public decimal ShippingPrice { get; set; }
    }
}