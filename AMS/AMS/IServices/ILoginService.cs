using AMS.Models;
using AMS.VMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.IServices
{
  public  interface ILoginService
    {
        ResponseMessage Login(VMLoginModel model);
    }
}
