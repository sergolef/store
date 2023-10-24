using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int statusCode, string message = null, string description = null) : base(statusCode, message)
        {
            Description = description;
        }

        public string Description { get; set;}
    }
}