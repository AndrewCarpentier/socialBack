using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialASPNET.ModelAndViews
{
    public class RegisterModelAndView
    {
        private List<string> errors;
        private string message;
        private bool success;

        public List<string> Errors { get => errors; set => errors = value; }
        public string Message { get => message; set => message = value; }
        public bool Success { get => success; set => success = value; }

        public RegisterModelAndView()
        {
            this.errors = new List<string>();
            this.success = false;
        }
    }
}
