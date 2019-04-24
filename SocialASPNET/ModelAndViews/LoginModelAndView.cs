using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialASPNET.ModelAndViews
{
    public class LoginModelAndView
    {
        private List<string> errors;
        private int id;
        private string username;
        private bool success;

        public LoginModelAndView()
        {
            errors = new List<string>();
        }

        public List<string> Errors { get => errors; set => errors = value; }
        public int Id { get => id; set => id = value; }
        public string Username { get => username; set => username = value; }
        public bool Success { get => success; set => success = value; }
    }
}
