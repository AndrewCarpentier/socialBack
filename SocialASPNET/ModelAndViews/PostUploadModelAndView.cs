using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialASPNET.ModelAndViews
{
    public class PostUploadModelAndView
    {
        private IFormFile file;
        private int idPost;

        public int IdPost { get => idPost; set => idPost = value; }
        public IFormFile File { get => file; set => file = value; }
    }
}
