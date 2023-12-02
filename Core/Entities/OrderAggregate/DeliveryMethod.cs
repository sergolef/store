using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities.OrderAggregate
{
    public class DeliveryMethod
    {
        public string ShortName { get; set; }
        public string DeliveryTime { get; set; }    
        public string Description { get; set; } 
        public decimal Price { get; set; }
    }
}