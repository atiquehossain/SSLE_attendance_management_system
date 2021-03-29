using AMS.IServices;
using AMS.Models;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeServices;
        public EmployeesController(IEmployeeService employeeServices)
        {
            _employeeServices = employeeServices;
        }

        [HttpGet]
       [Authorize(Roles = Roles.Admin)]
        public JsonResult Get()
        {
            var employee = _employeeServices.GetEmployees();
            return new JsonResult(employee);
        }
        [HttpPost]
       [Authorize(Roles = Roles.Admin)]
        public JsonResult Add([FromForm]TblEmployeeInformation employee)
        {

            var resuslt = _employeeServices.AddEmployee(employee);
            return new JsonResult(resuslt);
        }
        [HttpPut]
        public JsonResult Update([FromForm] TblEmployeeInformation employee)
        {

            var resuslt = _employeeServices.UpdateEmpolyee(employee);
            return new JsonResult(resuslt);
        }
        [HttpDelete]
        [Authorize(Roles = Roles.Admin)]
        public JsonResult Delete(int id)
        {

            var resuslt = _employeeServices.DeleteEmployee(id);
            return new JsonResult(resuslt);
        }

        [HttpGet("profile")]
        public JsonResult GetSingleProfile(int id)
        {

            var resuslt = _employeeServices.GetSingleProfile(id);
            return new JsonResult(resuslt);
        }

        [HttpPost("checkin")]
        public JsonResult CheckIn([FromForm] TblAttendance attendance)
        {

            var resuslt = _employeeServices.CheckIN(attendance);
            return new JsonResult(resuslt);
        }


       [HttpPost("checkout")]
        public JsonResult CheckOut(int id)
        {

            var resuslt = _employeeServices.CheckOut(id);
            return new JsonResult(resuslt);
        }


        [HttpDelete("deleteAttendance")]
       [Authorize(Roles = Roles.Admin)]
        public JsonResult DeleteAttendance(int id)
        {

            var resuslt = _employeeServices.DeleteAttendance(id);
            return new JsonResult(resuslt);
        }

        [HttpGet("allAttendance")]
       [Authorize(Roles = Roles.Admin)]
        public JsonResult GetAllAttendance()
        {

            var resuslt = _employeeServices.GetAllAttendance();
            return new JsonResult(resuslt);
        }

        [HttpPut("updateAttendance")]
       [Authorize(Roles = Roles.Admin)]
        public JsonResult UpdateAttendance([FromForm] TblAttendance attendance)
        {

            var resuslt = _employeeServices.UpdateAttendance(attendance);
            return new JsonResult(resuslt);
        }

        [HttpGet("searchAttendance")]
        public JsonResult SearchAttendance(int id)
        {
            var resuslt = _employeeServices.SearchAttendance(id);
            return new JsonResult(resuslt);
        }

        [HttpGet("searchAttendanceByDate")]
        public JsonResult SearchAttendanceByDate(int id, DateTime startDate, DateTime endDate)
        {
            var resuslt = _employeeServices.GetAttendanceByDate( id,  startDate,  endDate);
            return new JsonResult(resuslt);
        }

        [HttpGet("attendanceSummary")]
        [Authorize(Roles = Roles.Admin)]
        public JsonResult GetAttendanceSummary()
        {
            var resuslt = _employeeServices.GetAttendanceSummary();
            return new JsonResult(resuslt);
        }
    }
}
