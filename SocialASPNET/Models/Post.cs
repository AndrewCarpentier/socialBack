using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialASPNET.Models
{
    public class Post
    {
        private int id;
        private string description;
        private DateTime date;
        private List<string> imgs;
        private List<User> likes;
        private List<Comment> comments;

        public int Id { get => id; set => id = value; }
        public string Description { get => description; set => description = value; }
        public DateTime Date { get => date; set => date = value; }
        public List<string> Imgs { get => imgs; set => imgs = value; }
        public List<User> Likes { get => likes; set => likes = value; }
        public List<Comment> Comments { get => comments; set => comments = value; }
    }
}
