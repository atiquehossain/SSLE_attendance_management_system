using AMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.IServices
{
   public interface IEmployeeService
    {
        ResponseMessage  GetEmployees();
        ResponseMessage AddEmployee (TblEmployeeInformation employee);
        ResponseMessage UpdateEmpolyee (TblEmployeeInformation employee);
        ResponseMessage DeleteEmployee (int id);
        ResponseMessage GetSingleProfile(int id);
        ResponseMessage CheckIN(TblAttendance attendance);
        ResponseMessage CheckOut(int id);
        ResponseMessage DeleteAttendance(int id);
        ResponseMessage GetAllAttendance();
        ResponseMessage UpdateAttendance(TblAttendance attendance);
        ResponseMessage SearchAttendance(int id);
        ResponseMessage GetAttendanceByDate(int id, DateTime startDate, DateTime endDate);
        ResponseMessage GetAttendanceSummary();
    }
}
