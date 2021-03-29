using AMS.Constants;
using AMS.Enums;
using AMS.IServices;
using AMS.Models;
using AMS.Statics;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Services
{
    public class UpdateServices : IUpdateServices
    {
        private readonly string _dbConnectionString;
        OracleConnection _connection;
        OracleCommand _command;
        OracleDataReader _reader;
        public UpdateServices(IConfiguration configuration)
        {
            _dbConnectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public ResponseMessage GetEmployee()
        {
            throw new NotImplementedException();
        }

        public ResponseMessage UpdateEmpolyee()
        {
            ResponseMessage result = new ResponseMessage();
            List<TblEmployeeInformation> employeeList = new List<TblEmployeeInformation>();
            try
            {
                using (_connection = new OracleConnection(_dbConnectionString))
                {
                    using (_command = new OracleCommand())
                    {
                        _command.BindByName = true;
                        //_command.CommandText = "SCOTT.spLogin";
                        //_command.Parameters.Add("",)
                        _command.Connection = _connection;
                        _connection.Open();
                        _command.CommandText = "update TBLEMPLOYEEINFORMATION set CELL_NO = 01314567 where ID = 3";
                        _command.CommandType = CommandType.Text;
                        _reader = _command.ExecuteReader();

                        while (_reader.Read())
                        {
                            TblEmployeeInformation emp = new TblEmployeeInformation
                            {
                                ID = Convert.ToInt32(_reader["ID"]),
                                NAME = _reader["NAME"].ToString(),
                                DESIGNATION = _reader["DESIGNATION"].ToString(),
                                CELL_NO = _reader["CELL_NO"].ToString(),
                                EMAIL = _reader["EMAIL"].ToString(),
                                ADDRESS = _reader["ADDRESS"].ToString(),
                                NID = _reader["NID"].ToString(),
                                PASSWORD = _reader["PASSWORD"].ToString()
                            };
                            employeeList.Add(emp);
                        }

                        _connection.Close();
                    }
                }
                return result = ResponseMapping.GetResponseMessage(employeeList, (int)StatusCode.Success, ConstantMessage.RetriveSuccess);
            }
            catch (Exception ex)
            {

                return result = ResponseMapping.GetResponseMessage(null, (int)StatusCode.Faild, ex.Message.ToString());
            }
        }

    }
}
