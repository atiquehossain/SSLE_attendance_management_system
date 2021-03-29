using AMS.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateEmployeeController : ControllerBase
    {
        private readonly IUpdateServices _updateServices;
        public UpdateEmployeeController(IUpdateServices updateServices)
        {
            _updateServices = updateServices;
        }

        [HttpPut]
        public JsonResult Update()
        {
            var employee = _updateServices.UpdateEmpolyee();
            return new JsonResult(employee);
        }
    }
}
