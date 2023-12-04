using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class OrderOto
    {
        public string BasketId { get; set; }

        public int DeliveryMethodId { get; set; }

        public UserAddressDto ShipToAddress {get; set; }

    }
}