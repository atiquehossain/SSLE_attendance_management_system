using AMS.Constants;
using AMS.Encription;
using AMS.Enums;
using AMS.IServices;
using AMS.Models;
using AMS.Statics;
using AMS.VMModels;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Services
{
    public class LoginService:ILoginService
    {
        private readonly string _dbConnectionString;
        OracleConnection _connection;
        OracleCommand _command;
        OracleDataReader _reader;
        public LoginService(IConfiguration configuration)
        {
            _dbConnectionString= configuration.GetConnectionString("DefaultConnection");

        }

        public ResponseMessage Login(VMLoginModel model)
        {
            ResponseMessage result = new ResponseMessage();
            try
            {
                //string password = model.Password; // SimpleCryptService.Factory().Encrypt(model.Password);

                VMLoginResult employee = new VMLoginResult();

                using (_connection = new OracleConnection(_dbConnectionString))
                {
                    using (_command = new OracleCommand())
                    {
                        

                        _command.BindByName = true;
                        _command.Connection = _connection;
                        _connection.Open();
                        //_command.CommandText = $"select * from tblemployeeinformation where email='{model.Email}' and password='{model.Password}'";
                        _command.CommandText = "SP_LOGIN";
                        _command.Parameters.Add("p_email", model.Email);
                        _command.Parameters.Add("p_password", model.Password);
                        _command.Parameters.Add("p_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
                        _command.Parameters.Add("p_name", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                        _command.Parameters.Add("p_type_id", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;


                       /* OracleParameter pName = new OracleParameter();
                        pName.ParameterName = "@p_name";
                        pName.Direction = ParameterDirection.Output;
                        pName.OracleDbType = OracleDbType.Varchar2;
                        pName.Size = 4000;

                        OracleParameter pType = new OracleParameter();
                        pType.ParameterName = "@p_type_id";
                        pType.Direction = ParameterDirection.Output;
                        pType.OracleDbType = OracleDbType.Varchar2;
                        pType.Size = 4000;*/


                        _command.CommandType = CommandType.StoredProcedure;

                        int queryResult = _command.ExecuteNonQuery();

                        employee.Id = Convert.ToInt32(_command.Parameters["p_id"].Value.ToString());
                        employee.Name = _command.Parameters["p_name"].Value.ToString();
                        employee.Role = _command.Parameters["p_type_id"].Value.ToString();

                        /* _reader = _command.ExecuteReader();

                         while (_reader.Read())
                         {
                             employee.NAME = _reader["NAME"].ToString();
                             employee.EMAIL = _reader["EMAIL"].ToString();
                             employee.ID = Convert.ToInt32(_reader["ID"]);
                             employee.TYPE_ID = Convert.ToInt32(_reader["TYPE_ID"]);
                         }*/
                        _connection.Close();
                    }
                }

                if(employee.Id <= 0)
                {
                    return result = ResponseMapping.GetResponseMessage(null, (int)StatusCode.Faild, ConstantMessage.InvalidCreditional);
                }

                return result = ResponseMapping.GetResponseMessage(employee, (int)StatusCode.Success, ConstantMessage.LoginSuccess);

            }
            catch(Exception ex)
            {
                return result = ResponseMapping.GetResponseMessage(null, (int)StatusCode.Faild, ex.Message.ToString());
            }

        }

        
    }
}
