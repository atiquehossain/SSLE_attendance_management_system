using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Models
{
    public class ResponseMessage
    {
        public object Data { get; set; }
        public bool IsSuccess  { get; set; }
        public string Message { get; set; }
    }
}
