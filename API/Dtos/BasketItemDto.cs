using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class BasketItemDto
    {

        [Required]
        public int Id { get; set; } 
        
        [Required]
        public string ProductName { get; set; }

        [Required]
         [Range(0.1, double.MaxValue, ErrorMessage = "Price can't be zerro")]
        public decimal Price { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Type { get; set; }


        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Quantity can't be zerro")]
        public int Quantity { get; set; }

        [Required]
        public string PictureUrl { get; set; }
    }
}