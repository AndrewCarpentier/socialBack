using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialASPNET.Models
{
    public class User
    {
        private int id;
        private string username;
        private string fullname;
        private string email;
        private string phone;
        private string password;
        private string description;
        private string urlImgProfil;
        private bool? sexe;
        private List<Post> posts;
        private List<User> subcribers;
        private List<User> subscriptions;

        public int Id { get => id; set => id = value; }
        public string Username { get => username; set => username = value; }
        public string Fullname { get => fullname; set => fullname = value; }
        public string Email { get => email; set => email = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Password { get => password; set => password = value; }
        public string Description { get => description; set => description = value; }
        public string UrlImgProfil { get => urlImgProfil; set => urlImgProfil = value; }
        public bool? Sexe { get => sexe; set => sexe = value; }
        public List<Post> Posts { get => posts; set => posts = value; }
        public List<User> Subcribers { get => subcribers; set => subcribers = value; }
        public List<User> Subscriptions { get => subscriptions; set => subscriptions = value; }

        public User()
        {
            posts = new List<Post>();
            subcribers = new List<User>();
            subscriptions = new List<User>();
        }
    }
}
