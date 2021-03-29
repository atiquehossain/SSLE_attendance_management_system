using AMS.IServices;
using AMS.Models;
using AMS.VMModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using AMS.Statics;
using AMS.Constants;

namespace AMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly string _jwtTokenKey;
        public LoginController(IConfiguration configuration, ILoginService loginService)
        {
            _loginService = loginService;
            _jwtTokenKey= configuration.GetValue<string>("JWT");
        }

        [HttpPost]
        public async Task<IActionResult> Login(VMLoginModel model)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                var response = _loginService.Login(model);

                if (response.IsSuccess == true)
                {
                    var employee = (VMLoginResult)(response.Data);

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenKey = Encoding.ASCII.GetBytes(_jwtTokenKey);
                    
                    var tokenDescription = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name, employee.Name),
                        new Claim(ClaimTypes.Sid, employee.Id.ToString()),
                        new Claim(ClaimTypes.Role, employee.Role.ToString()),
                    }),

                        Expires = DateTime.UtcNow.AddDays(1),

                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(tokenKey),
                            SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenDetails = tokenHandler.CreateToken(tokenDescription);

                    var jwtToken = tokenHandler.WriteToken(tokenDetails);

                    result = ResponseMapping.GetResponseMessage(jwtToken, 1, ConstantMessage.LoginSuccess);
                    return new JsonResult(result);
                }

                result = ResponseMapping.GetResponseMessage(null, 2, response.Message);

            }
            catch(Exception ex)
            {
                result = ResponseMapping.GetResponseMessage(null, 2, ex.Message.ToString());
            }

            return new JsonResult(result);
        }
    }
}
