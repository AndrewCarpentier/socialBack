using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialASPNET.Database;
using SocialASPNET.ModelAndViews;
using SocialASPNET.Models;
using SocialASPNET.Utils;

namespace SocialASPNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserDatabase userDatabase = new UserDatabase();
        private PasswordEncoder passwordEncoder = new PasswordEncoder();

        [HttpPost][EnableCors("AllowMyOrigin")]
        [Route("register")]
        public JsonResult register(User user)
        {
            RegisterModelAndView result = new RegisterModelAndView();
            if (user.Email.Equals(""))
            {
                result.Errors.Add("email is null");
            }
            if (user.Fullname.Equals(""))
            {
                result.Errors.Add("fullname is null");
            }
            if (user.Username.Equals(""))
            {
                result.Errors.Add("username is null");
            }
            if (user.Password.Equals(""))
            {
                result.Errors.Add("password is null");
            }
            if (userDatabase.RegisterVerificationEmail(user.Email))
            {
                result.Errors.Add("this email is already used");
            }
            if (userDatabase.RegisterVerificationUsername(user.Username))
            {
                result.Errors.Add("this username is already used");
            }

            if(result.Errors.Count == 0)
            {
                try
                {
                    user.Password = passwordEncoder.Encode(user.Password);
                    userDatabase.Register(user);

                    result.Message = "Register is a success";
                    result.Success = true;
                }
                catch
                {
                    result.Message = "Problem with dataBase";
                }
            }
            else
            {
                result.Message = "Register is not a success";
            }
            return new JsonResult(result);
        }

        //[HttpPost][EnableCors("AllowMyOrigin")]
        //[Route("login")]
        //public JsonResult login(User user)
        //{
        //}
    }
}