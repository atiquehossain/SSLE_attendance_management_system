using AMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Statics
{
    public class ResponseMapping
    {
        public static ResponseMessage AddResponseMessage( int statusCode, string message)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            //responseMessage.Data = data;
            responseMessage.IsSuccess = statusCode == 1 ? true : false;
            responseMessage.Message = message;
            return responseMessage;
        }
        public static ResponseMessage GetResponseMessage(object data,int statusCode,string message)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            responseMessage.Data = data;
            responseMessage.IsSuccess = statusCode == 1 ? true : false;
            responseMessage.Message = message;
            return responseMessage;
        }
    }
}
