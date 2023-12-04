using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrderWithItemsAndOrderingSpecification : BaseSpecification<Order>
    {
        public OrderWithItemsAndOrderingSpecification(string email): base(o => o.BuyerEmail == email)
        {
            AddIncludes(o => o.OrderItems);
            AddIncludes(o => o.DeliveryMethod);
            AddOrderByDescending(o => o.OrderDate);
        }

        public OrderWithItemsAndOrderingSpecification(int id, string email) : base(o => o.Id == id && o.BuyerEmail == email)
        {
            
            AddIncludes(o => o.OrderItems);
            AddIncludes(o => o.DeliveryMethod);
        }
    }
}