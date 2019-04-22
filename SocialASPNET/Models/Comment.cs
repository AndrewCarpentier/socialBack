using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialASPNET.Models
{
    public class Comment
    {
        private int id;
        private string description;
        private User user;
        private DateTime date;
        private List<User> likes;

        public int Id { get => id; set => id = value; }
        public string Description { get => description; set => description = value; }
        public User User { get => user; set => user = value; }
        public DateTime Date { get => date; set => date = value; }
        public List<User> Likes { get => likes; set => likes = value; }
    }
}
