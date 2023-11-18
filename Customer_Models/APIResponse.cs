using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Customer_Models
{
    public class APIResponse<T>
    {
        public T Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; } = true;


    }
}
