using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate
{
    public class ProductItemOrdered
    {
        public ProductItemOrdered()
        {
        }


        public ProductItemOrdered(int productItemId, string productName, string pictureUrl) 
        {
            ProductItemId = productItemId;
            PictureUrl = pictureUrl;
            ProductName = productName;
   
        }
        public int ProductItemId { get; set; }  
        public string ProductName { get; set; } 
        public string PictureUrl { get; set; }

        
    }


}