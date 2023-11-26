using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("errors/{id}")]
    public class ErrorController :BaseApiController
    {
        
        public IActionResult Error(int? id = null){
            
            var st = id.HasValue ? id.Value : 500;
            return new ObjectResult(new ApiResponse(st));
        }
    }
}