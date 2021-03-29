using AMS.Constants;
using AMS.Encription;
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
    public class EmployeeServices : IEmployeeService
    {
        private readonly string _dbConnectionString;
        OracleConnection _connection;
        OracleCommand _command;
        OracleDataReader _reader;
        public EmployeeServices(IConfiguration configuration)
        {
            _dbConnectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public ResponseMessage AddEmployee(TblEmployeeInformation employee)
        {
            ResponseMessage result = new ResponseMessage();
            int sqlResult = 0;

            try
            {
                string password = SimpleCryptService.Factory().Encrypt(employee.PASSWORD);

                using (_connection = new OracleConnection(_dbConnectionString))
                {
                    using (_command = new OracleCommand())
                    {
                        _command.BindByName = true;
                        _command.Connection = _connection;
                        _connection.Open();
                        _command.CommandText = "spAddProfile";
                        _command.Parameters.Add("p_name", employee.NAME);
                        _command.Parameters.Add("p_designation", employee.DESIGNATION);
                        _command.Parameters.Add("p_cell_no", employee.CELL_NO);
                        _command.Parameters.Add("p_email", employee.EMAIL);
                        _command.Parameters.Add("p_address", employee.ADDRESS);
                        _command.Parameters.Add("p_nid", employee.NID);
                        _command.Parameters.Add("p_password", password);
                        _command.Parameters.Add("p_type", employee.TYPE_ID);
                        // _command.CommandText = $"insert into TBLEMPLOYEEINFORMATION(ID,NAME,DESIGNATION,CELL_NO,EMAIL,ADDRESS,NID,PASSWORD) values ({employee.ID},'{employee.NAME}','{employee.DESIGNATION}','{employee.CELL_NO}','{employee.EMAIL}','{employee.ADDRESS}','{employee.NID}','{employee.PASSWORD}')";
                        // _command.CommandText = $"insert into TBLEMPLOYEEINFORMATION(ID,NAME,DESIGNATION,CELL_NO,EMAIL,ADDRESS,NID,PASSWORD) values ("+employee.ID+",'"+employee.NAME+"','"+ employee.DESIGNATION + "','" + employee.CELL_NO + "','" + employee.EMAIL + "','" + employee.ADDRESS + "','" + employee.NID + "','" + employee.PASSWORD + "')";
                        _command.CommandType = CommandType.StoredProcedure;
                        sqlResult = _command.ExecuteNonQuery();
                        _connection.Close();
                    }
                }
                return result = ResponseMapping.GetResponseMessage(sqlResult, (int)StatusCode.Success, ConstantMessage.AddProfileSuccess);
            }
            catch (Exception ex)
            {

                return result = ResponseMapping.AddResponseMessage((int)StatusCode.Faild, ex.Message.ToString());
            }
        }

        public ResponseMessage DeleteEmployee(int id)
        {
            ResponseMessage result = new ResponseMessage();
            int sqlResult = 0;

            try
            {
                using (_connection = new OracleConnection(_dbConnectionString))
                {
                    using (_command = new OracleCommand())
                    {
                        _command.BindByName = true;
                        _command.Connection = _connection;
                        _connection.Open();
                        _command.CommandText = "spDeleteProfile";
                        _command.Parameters.Add("P_ID", id);
                        //_command.Parameters.Add("P_MSG", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                        _command.CommandType = CommandType.StoredProcedure;
                        sqlResult = _command.ExecuteNonQuery();
                        _connection.Close();
                    }
                }
                return result = ResponseMapping.GetResponseMessage(sqlResult, (int)StatusCode.Success, ConstantMessage.DeleteSuccess);
            }
            catch (Exception ex)
            {

                return result = ResponseMapping.AddResponseMessage((int)StatusCode.Faild, ex.Message.ToString());
            }
        }


        public ResponseMessage GetEmployees()
        {
            ResponseMessage result = new ResponseMessage();
            List<TblEmployeeInformation> employeeList = new List<TblEmployeeInformation>();
            //int sqlResult = 0;
            try
            {
                using (_connection = new OracleConnection(_dbConnectionString))
                {
                    using (_command = new OracleCommand())
                    {
                        _command.BindByName = true;
                        _connection.Open();
                        _command.Connection = _connection;
                        /*_command.CommandText = "spFetchAllProfile";
                         _command.Parameters.Add("p_id", OracleDbType.Int64).Direction = ParameterDirection.Output;
                         _command.Parameters.Add("p_name", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                         _command.Parameters.Add("p_designation", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                         _command.Parameters.Add("p_cell_no", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                         _command.Parameters.Add("p_email", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                         _command.Parameters.Add("p_address", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                         _command.Parameters.Add("p_nid", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                         _command.Parameters.Add("p_password", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                         _command.Parameters.Add("p_type_id", OracleDbType.Int64).Direction = ParameterDirection.Output;
                         _command.Parameters.Add("po_message", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;*/

                        _command.CommandText = "sp_FetchAllProfile";
                        _command.Parameters.Add("employee_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        // _command.CommandText = "spFetchAllProfile";
                        // _command.Parameters.Add("c1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                        // _command.Parameters.Add("p_type", employee.TYPE);
                        // _command.Parameters.Add("po_message", OracleDbType.Varchar2, 1000).Direction = ParameterDirection.Output;
                        _command.CommandType = CommandType.StoredProcedure;
                        // _command.ExecuteNonQuery();
                        //_connection.Close();

                        //_command.CommandText = "select *from TBLEMPLOYEEINFORMATION";
                        //_command.CommandType = CommandType.Text;
                        _reader = _command.ExecuteReader();

                        while (_reader.Read())
                        {
                            TblEmployeeInformation employee = new TblEmployeeInformation();

                            employee.ID = Convert.ToInt32(_reader["ID"]);
                            employee.NAME = _reader["NAME"].ToString();
                            employee.DESIGNATION = _reader["DESIGNATION"].ToString();
                            employee.CELL_NO = _reader["CELL_NO"].ToString();
                            employee.EMAIL = _reader["EMAIL"].ToString();
                            employee.ADDRESS = _reader["ADDRESS"].ToString();
                            employee.NID = _reader["NID"].ToString();
                            employee.PASSWORD = _reader["PASSWORD"].ToString();
                            employee.TYPE_ID = _reader["TYPE_ID"].ToString();
                            //employee.TYPE_ID = Convert.ToInt32(_reader["TYPE_ID"]);

                            employeeList.Add(employee);

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

        public ResponseMessage UpdateEmpolyee(TblEmployeeInformation employee)
        {
            ResponseMessage result = new ResponseMessage();
            int sqlResult = 0;

            try
            {
                using (_connection = new OracleConnection(_dbConnectionString))
                {
                    using (_command = new OracleCommand())
                    {
                        _command.BindByName = true;
                        _command.Connection = _connection;
                        _connection.Open();
                        _command.CommandText = "spUpdateInformation";
                        _command.Parameters.Add("p_id", employee.ID);
                        _command.Parameters.Add("p_name", employee.NAME);
                        _command.Parameters.Add("p_designation", employee.DESIGNATION);
                        _command.Parameters.Add("p_cell_no", employee.CELL_NO);
                        _command.Parameters.Add("p_email", employee.EMAIL);
                        _command.Parameters.Add("p_address", employee.ADDRESS);
                        _command.Parameters.Add("p_nid", employee.NID);
                        _command.Parameters.Add("p_password", employee.PASSWORD);
                        _command.Parameters.Add("p_type", employee.TYPE_ID);
                        _command.CommandType = CommandType.StoredProcedure;
                        sqlResult = _command.ExecuteNonQuery();
                        _connection.Close();
                    }
                }
                return result = ResponseMapping.GetResponseMessage(sqlResult, (int)StatusCode.Success, ConstantMessage.UpdateSuccess);
            }
            catch (Exception ex)
            {

                return result = ResponseMapping.AddResponseMessage((int)StatusCode.Faild, ex.Message.ToString());
            }
        }

        public ResponseMessage GetSingleProfile(int id)
        {
            ResponseMessage result = new ResponseMessage();
            //List<TblEmployeeInformation> employeeList = new List<TblEmployeeInformation>();
            try
            {
                TblEmployeeInformation employee = new TblEmployeeInformation();
                using (_connection = new OracleConnection(_dbConnectionString))
                {
                    using (_command = new OracleCommand())
                    {
                        _command.BindByName = true;
                        _connection.Open();
                        _command.Connection = _connection;

                        _command.CommandText = "spFetchSingleProfile";
                        _command.Parameters.Add("p_id", id);
                        _command.Parameters.Add("o_name", OracleDbType.Varchar2, 1000).Direction = ParameterDirection.Output;
                        _command.Parameters.Add("o_designation", OracleDbType.Varchar2, 1000).Direction = ParameterDirection.Output;
                        _command.Parameters.Add("o_cell_no", OracleDbType.Varchar2, 1000).Direction = ParameterDirection.Output;
                        _command.Parameters.Add("o_email", OracleDbType.Varchar2, 1000).Direction = ParameterDirection.Output;
                        _command.Parameters.Add("o_address", OracleDbType.Varchar2, 1000).Direction = ParameterDirection.Output;
                        _command.Parameters.Add("o_nid", OracleDbType.Varchar2, 1000).Direction = ParameterDirection.Output;
                        _command.Parameters.Add("o_password", OracleDbType.Varchar2, 1000).Direction = ParameterDirection.Output;
                        _command.Parameters.Add("o_type_id", OracleDbType.Varchar2, 1000).Direction = ParameterDirection.Output;

                        _command.CommandType = CommandType.StoredProcedure;
                        int queryResult = _command.ExecuteNonQuery();

                        //employee.ID = Convert.ToInt32(_command.Parameters["P_ID"].Value.ToString());
                        employee.ID = id;
                        employee.NAME = _command.Parameters["o_name"].Value.ToString();
                        employee.DESIGNATION = _command.Parameters["o_designation"].Value.ToString();
                        employee.CELL_NO = _command.Parameters["o_cell_no"].Value.ToString();
                        employee.EMAIL = _command.Parameters["o_email"].Value.ToString();
                        employee.ADDRESS = _command.Parameters["o_address"].Value.ToString();
                        employee.NID = _command.Parameters["o_nid"].Value.ToString();
                        employee.PASSWORD = _command.Parameters["o_password"].Value.ToString();
                        employee.TYPE_ID = _command.Parameters["o_type_id"].Value.ToString();

                        _connection.Close();

                    }
                }


                return result = ResponseMapping.GetResponseMessage(employee, (int)StatusCode.Success, ConstantMessage.RetriveSuccess);
            }
            catch (Exception ex)
            {

                return result = ResponseMapping.GetResponseMessage(null, (int)StatusCode.Faild, ex.Message.ToString());
            }
        }

        public ResponseMessage CheckIN(TblAttendance attendance)
        {
            ResponseMessage result = new ResponseMessage();
            int sqlResult = 0;

            try
            {

                using (_connection = new OracleConnection(_dbConnectionString))
                {
                    using (_command = new OracleCommand())
                    {
                        _command.BindByName = true;
                        _command.Connection = _connection;
                        _connection.Open();
                        _command.CommandText = "spCheckIn";
                        _command.Parameters.Add("p_employee_id", attendance.EMPLOYEE_ID);
                        _command.Parameters.Add("p_latitude", attendance.LATITUDE);
                        _command.Parameters.Add("p_longitude", attendance.LONGITUDE);
                        _command.CommandType = CommandType.StoredProcedure;
                        sqlResult = _command.ExecuteNonQuery();
                        _connection.Close();
                    }
                }
                return result = ResponseMapping.GetResponseMessage(sqlResult, (int)StatusCode.Success, ConstantMessage.CheckInSuccess);
            }
            catch (Exception ex)
            {

                return result = ResponseMapping.GetResponseMessage(null, (int)StatusCode.Faild, ex.Message.ToString());
            }
        }

        public ResponseMessage CheckOut(int id)
        {
            ResponseMessage result = new ResponseMessage();
            int sqlResult = 0;

            try
            {

                using (_connection = new OracleConnection(_dbConnectionString))
                {
                    using (_command = new OracleCommand())
                    {
                        _command.BindByName = true;
                        _command.Connection = _connection;
                        _connection.Open();
                        _command.CommandText = "spCHECK_OUT";
                        _command.Parameters.Add("p_id", id);
                        _command.CommandType = CommandType.StoredProcedure;
                        sqlResult = _command.ExecuteNonQuery();
                        _connection.Close();
                    }
                }
                return result = ResponseMapping.GetResponseMessage(sqlResult, (int)StatusCode.Success, ConstantMessage.CheckOutSuccess);
            }
            catch (Exception ex)
            {

                return result = ResponseMapping.GetResponseMessage(null, (int)StatusCode.Faild, ex.Message.ToString());
            }
        }

        public ResponseMessage DeleteAttendance(int id)
        {
            ResponseMessage result = new ResponseMessage();
            int sqlResult = 0;

            try
            {

                using (_connection = new OracleConnection(_dbConnectionString))
                {
                    using (_command = new OracleCommand())
                    {
                        _command.BindByName = true;
                        _command.Connection = _connection;
                        _connection.Open();
                        _command.CommandText = "spDeleteAttendance";
                        _command.Parameters.Add("P_ID", id);
                        _command.CommandType = CommandType.StoredProcedure;
                        sqlResult = _command.ExecuteNonQuery();
                        _connection.Close();
                    }
                }
                return result = ResponseMapping.GetResponseMessage(sqlResult, (int)StatusCode.Success, ConstantMessage.DeleteAttendanceSuccess);
            }
            catch (Exception ex)
            {

                return result = ResponseMapping.GetResponseMessage(null, (int)StatusCode.Faild, ex.Message.ToString());
            }
        }

        public ResponseMessage GetAllAttendance()
        {
            ResponseMessage result = new ResponseMessage();
            List<TblAttendance> attendanceList = new List<TblAttendance>();

            try
            {
                using (_connection = new OracleConnection(_dbConnectionString))
                {
                    using (_command = new OracleCommand())
                    {
                        _connection.Open();
                        _command.Connection = _connection;

                        _command.CommandText = "sp_FetchAllAttendance";
                        _command.Parameters.Add("attendance_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        _command.CommandType = CommandType.StoredProcedure;
                        _reader = _command.ExecuteReader();

                        while (_reader.Read())
                        {
                            TblAttendance attendance = new TblAttendance();

                            attendance.ID = Convert.ToInt32(_reader["ID"]);
                            attendance.DATE_TIME = Convert.ToDateTime(_reader["DATE_TIME"].ToString());
                            attendance.CHECK_IN = Convert.ToDateTime(_reader["CHECK_IN"]);
                            attendance.CHECK_OUT = (!string.IsNullOrEmpty(_reader["CHECK_OUT"].ToString())) ? Convert.ToDateTime(_reader["CHECK_OUT"]) : null;
                            attendance.LATE_DURATION = Convert.ToInt32(_reader["LATE_DURATION"]);
                            attendance.STATUS = _reader["STATUS"].ToString();
                            attendance.LATITUDE = Convert.ToDouble(_reader["LATITUDE"]);
                            attendance.LONGITUDE = Convert.ToDouble(_reader["LONGITUDE"]);

                            attendanceList.Add(attendance);

                        }

                        _connection.Close();

                    }

                }


                return result = ResponseMapping.GetResponseMessage(attendanceList, (int)StatusCode.Success, ConstantMessage.RetriveSuccess);
            }
            catch (Exception ex)
            {

                return result = ResponseMapping.GetResponseMessage(null, (int)StatusCode.Faild, ex.Message.ToString());
            }
        }

        public ResponseMessage UpdateAttendance(TblAttendance attendance)
        {
            ResponseMessage result = new ResponseMessage();
            int sqlResult = 0;

            try
            {

                using (_connection = new OracleConnection(_dbConnectionString))
                {
                    using (_command = new OracleCommand())
                    {
                        _command.BindByName = true;
                        _command.Connection = _connection;
                        _connection.Open();
                        _command.CommandText = "spUpdateAttendance";
                        _command.Parameters.Add("p_id", attendance.ID);
                        _command.Parameters.Add("p_check_in", attendance.CHECK_IN);
                        _command.Parameters.Add("p_check_out", attendance.CHECK_OUT);
                        _command.Parameters.Add("p_status", attendance.STATUS);
                        _command.CommandType = CommandType.StoredProcedure;
                        sqlResult = _command.ExecuteNonQuery();
                        _connection.Close();
                    }
                }
                return result = ResponseMapping.GetResponseMessage(sqlResult, (int)StatusCode.Success, ConstantMessage.UpdateAttendanceSuccess);
            }
            catch (Exception ex)
            {

                return result = ResponseMapping.GetResponseMessage(null, (int)StatusCode.Faild, ex.Message.ToString());
            }
        }

        public ResponseMessage SearchAttendance(int id)
        {
            ResponseMessage result = new ResponseMessage();
            List<TblAttendance> attendanceList = new List<TblAttendance>();

            try
            {
                using (_connection = new OracleConnection(_dbConnectionString))
                {
                    using (_command = new OracleCommand())
                    {
                        _command.BindByName = true;
                        _connection.Open();
                        _command.Connection = _connection;
                        _command.CommandText = "spSearchAttendance";
                        _command.Parameters.Add("p_id", id);
                        _command.Parameters.Add("EMP_ATTEN_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        _command.CommandType = CommandType.StoredProcedure;
                        _reader = _command.ExecuteReader();

                        while (_reader.Read())
                        {
                            TblAttendance attendance = new TblAttendance();

                            attendance.ID = Convert.ToInt32(_reader["ID"]); ;
                            attendance.EMPLOYEE_ID = id;
                            attendance.DATE_TIME = Convert.ToDateTime(_reader["DATE_TIME"]);
                            attendance.CHECK_IN = Convert.ToDateTime(_reader["CHECK_IN"]);
                            attendance.CHECK_OUT = Convert.ToDateTime(_reader["CHECK_OUT"]);
                            attendance.LATE_DURATION = Convert.ToInt32(_reader["LATE_DURATION"]);
                            attendance.STATUS = _reader["STATUS"].ToString();

                            attendanceList.Add(attendance);

                        }

                        _connection.Close();

                    }

                }


                return result = ResponseMapping.GetResponseMessage(attendanceList, (int)StatusCode.Success, ConstantMessage.RetriveSuccess);
            }
            catch (Exception ex)
            {

                return result = ResponseMapping.GetResponseMessage(null, (int)StatusCode.Faild, ex.Message.ToString());
            }
        }


        public ResponseMessage GetAttendanceByDate(int id, DateTime startDate, DateTime endDate)
        {
            ResponseMessage result = new ResponseMessage();
            List<TblAttendance> attendanceList = new List<TblAttendance>();

            try
            {
                using (_connection = new OracleConnection(_dbConnectionString))
                {
                    using (_command = new OracleCommand())
                    {
                        _command.BindByName = true;
                        _connection.Open();
                        _command.Connection = _connection;

                        _command.CommandText = "spSearchByDate";
                        _command.Parameters.Add("p_id", id);
                        _command.Parameters.Add("start_date", startDate);
                        _command.Parameters.Add("end_date", endDate);
                        _command.Parameters.Add("EMP_ATTEN_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        _command.CommandType = CommandType.StoredProcedure;
                        _reader = _command.ExecuteReader();

                        while (_reader.Read())
                        {
                            TblAttendance attendance = new TblAttendance();

                            attendance.ID = Convert.ToInt32(_reader["ID"]);
                            attendance.EMPLOYEE_ID = id;
                            attendance.DATE_TIME = Convert.ToDateTime(_reader["DATE_TIME"]);
                            attendance.CHECK_IN = Convert.ToDateTime(_reader["CHECK_IN"]);
                            attendance.CHECK_OUT = Convert.ToDateTime(_reader["CHECK_OUT"]);
                            attendance.LATE_DURATION = Convert.ToInt32(_reader["LATE_DURATION"]);
                            attendance.STATUS = _reader["STATUS"].ToString();

                            attendanceList.Add(attendance);

                        }

                        _connection.Close();

                    }

                }


                return result = ResponseMapping.GetResponseMessage(attendanceList, (int)StatusCode.Success, ConstantMessage.RetriveSuccess);
            }
            catch (Exception ex)
            {

                return result = ResponseMapping.GetResponseMessage(null, (int)StatusCode.Faild, ex.Message.ToString());
            }
        }

        public ResponseMessage GetAttendanceSummary()
        {
            ResponseMessage result = new ResponseMessage();
            //List<TblAttendance> attendanceList = new List<TblAttendance>();
            //List<TblEmployeeInformation> employeeList = new List<TblEmployeeInformation>();
            AttendanceSummary summary = new AttendanceSummary();

            try
            {
                using (_connection = new OracleConnection(_dbConnectionString))
                {
                    using (_command = new OracleCommand())
                    {
                        _command.BindByName = true;
                        _connection.Open();
                        _command.Connection = _connection;

                        _command.CommandText = "spAttendanceSummary";
                        //_command.Parameters.Add();
                        
                        _command.Parameters.Add("EMP_ATTEN_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        _command.CommandType = CommandType.StoredProcedure;
                        _reader = _command.ExecuteReader();

                        while (_reader.Read())
                        {

                            summary.totalEmployee = Convert.ToInt32(_reader["TOTAL_EMPLOYEE"]);
                            summary.totalPresent = Convert.ToInt32(_reader["TOTAL_PRESENT"]);
                            summary.totalAbsent = Convert.ToInt32(_reader["TOTAL_ABSENT"]);
                            
                            

                            //attendanceList.Add(attendance);

                        }

                        _connection.Close();

                    }

                }


                return result = ResponseMapping.GetResponseMessage(summary, (int)StatusCode.Success, ConstantMessage.RetriveSuccess);
            }
            catch (Exception ex)
            {

                return result = ResponseMapping.GetResponseMessage(null, (int)StatusCode.Faild, ex.Message.ToString());
            }
        }

    }
}
