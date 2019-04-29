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
        public JsonResult Register(User user)
        {
            RegisterModelAndView result = new RegisterModelAndView();
            if (user.Email.Equals(""))
                result.Errors.Add("email is null");
            if (user.Fullname.Equals(""))
                result.Errors.Add("fullname is null");
            if (user.Username.Equals(""))
                result.Errors.Add("username is null");
            if (user.Password.Equals(""))
                result.Errors.Add("password is null");
            if (userDatabase.RegisterVerificationEmail(user.Email))
                result.Errors.Add("this email is already used");
            if (userDatabase.RegisterVerificationUsername(user.Username))
                result.Errors.Add("this username is already used");

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
                result.Message = "Register is not a success";

            return new JsonResult(result);
        }

        [HttpPost][EnableCors("AllowMyOrigin")]
        [Route("login")]
        public JsonResult Login(User user)
        {
            LoginModelAndView result = new LoginModelAndView();
            if (user.Email.Equals(""))
                result.Errors.Add("Email is null");
            if (user.Password.Equals(""))
                result.Errors.Add("Password is null");

            user.Password = passwordEncoder.Encode(user.Password);
            user = userDatabase.Login(user);

            if(user.Id != 0)
            {
                result.Id = user.Id;
                result.Username = user.Username;
                result.Success = true;
            }
            else
            {
                result.Success = false;
            }

            return new JsonResult(result);
        }

        [HttpGet][EnableCors("AllowMyOrigin")]
        [Route("getusername")]
        public JsonResult GetUsername(string username)
        {
            User user = new User();
            user = userDatabase.GetUserByUsername(username);
            user = userDatabase.GetUserSubscriber(user);
            user = userDatabase.GetUserSubscriptions(user);
            return new JsonResult(user);
        }

        [HttpPost][EnableCors("AllowMyOrigin")]
        [Route("upload")]
        public JsonResult Upload(IFormFile[] files)
        {
            return new JsonResult("test");
        }
    }
}