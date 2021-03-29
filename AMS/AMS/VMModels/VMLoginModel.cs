using AMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.VMModels
{
    public class VMLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsRememberMe { get; set; }
    }
}
