using System;
using System.Collections.Generic;
using System.IO;
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

        [Route("register"), HttpPost, EnableCors("AllowMyOrigin")]
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

        [Route("login"), HttpPost, EnableCors("AllowMyOrigin")]
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

        [Route("getusername"), HttpGet, EnableCors("AllowMyOrigin")]
        public JsonResult GetUsername(string username)
        {
            User user = new User();
            user = userDatabase.GetUserByUsername(username);
            if(user.Id != 0)
            {
                user = userDatabase.GetUserSubscriber(user);
                user = userDatabase.GetUserSubscriptions(user);
            }
            
            return new JsonResult(user);
        }

        [Route("subscribe"), HttpPost, EnableCors("AllowMyOrigin")]
        public JsonResult Subscribe(Subscribe s)
        {
            userDatabase.Subscribe(s);
            return new JsonResult("Subscribe success");
        }

        [Route("unsubscribe"), HttpPost, EnableCors("AllowMyOrigin")]
        public JsonResult Unsubscribe(Subscribe s)
        {
            userDatabase.Unsubscribe(s);
            return new JsonResult("Unsubscribe success");
        }

        [Route("verifSubscribed"), HttpPost, EnableCors("AllowMyOrigin")]
        public JsonResult VerifSubscribed(Subscribe s)
        {
            return new JsonResult(userDatabase.VerifSubscribed(s));
        }

        [Route("upload/{id}"), HttpPost, EnableCors("AllowMyOrigin")]
        public JsonResult Upload(List<IFormFile> files, int id, string description)
        {
            Post p = new Post()
            {
                Description = description,
                IdUser = id
            };

            userDatabase.UploadPost(p, files);

            return new JsonResult("test");
        }

        [Route("uploadProfilImg"), HttpPost, EnableCors("AllowMyOrigin")]
        public async Task<JsonResult> UploadProfilImg(IFormFile file, int id)
        {
            string fileName = "0";
            if(file != null && file.Length != 0)
            {
                fileName = $"{id}-{file.FileName}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
                var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);
                userDatabase.UploadProfilImg($"http://localhost:50255/api/user/img/{fileName}", id);
            }

            return new JsonResult($"http://localhost:50255/api/user/img/{fileName}");
        }

        [Route("img/{filename}"), HttpGet, EnableCors("AllowMyOrigin")]
        public ActionResult Image(string filename)
        {
            var path = Path.Combine(@"~\img\" + filename);
            return base.File(path, "image/jpeg");
        }

        //[Route("img"), HttpGet, EnableCors("AllowMyOrigin")]
        //public IActionResult GetImage()
        //{
        //    var image = System.IO.File.OpenRead(@"~/img/test.jpg");
        //    return File(image, "image/jpeg");
        //}

    }
}