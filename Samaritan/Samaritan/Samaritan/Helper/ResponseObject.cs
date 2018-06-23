using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Samaritan.Helper
{
    public class ResponseObject<T>
    {
        public T Response { get; set; }

        public string ErrorMessage { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
