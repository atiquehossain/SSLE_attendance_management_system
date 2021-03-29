using AMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.IServices
{
    public interface IUpdateServices
    {
        public ResponseMessage GetEmployee();
       
        //ResponseMessage AddEmployee(TblEmployeeInformation employee);
        public ResponseMessage UpdateEmpolyee();
        //ResponseMessage DeleteEmployee(int id);
    }
}
