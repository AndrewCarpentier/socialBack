using Microsoft.AspNetCore.Http;
using SocialASPNET.ModelAndViews;
using SocialASPNET.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialASPNET.Database
{
    public class UserDatabase
    {
        private static Mutex uploadPostMutex = new Mutex();

        public void Register(User user)
        {
            SqlCommand command = new SqlCommand(
                "INSERT INTO users (email, fullname, username, password) VALUES (@email, @fullname, @username, @password)",
                Connection.Instance);
            command.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar) { Value = user.Email });
            command.Parameters.Add(new SqlParameter("@fullname", SqlDbType.VarChar) { Value = user.Fullname });
            command.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar) { Value = user.Username });
            command.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar) { Value = user.Password });
            Connection.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            Connection.Instance.Close();
        }

        public bool RegisterVerificationEmail(string email)
        {
            bool find = false;
            SqlCommand command = new SqlCommand(
                "SELECT id FROM users WHERE email = @email ", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar) { Value = email });
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                find = true;
            }

            command.Dispose();
            reader.Close();
            Connection.Instance.Close();

            return find;
        }

        public bool RegisterVerificationUsername(string username)
        {
            bool find = false;
            SqlCommand command = new SqlCommand(
                "SELECT id FROM users WHERE username = @username ", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar) { Value = username });
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                find = true;
            }

            command.Dispose();
            reader.Close();
            Connection.Instance.Close();

            return find;
        }

        public User Login(User user)
        {

            SqlCommand command = new SqlCommand(
                "SELECT id, username FROM users WHERE email = @email AND password = @password"
                , Connection.Instance);
            command.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar) { Value = user.Email });
            command.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar) { Value = user.Password });
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                user.Id = reader.GetInt32(0);
                user.Username = reader.GetString(1);
            }

            command.Dispose();
            reader.Close();
            Connection.Instance.Close();

            return user;
        }

        public User GetUserByUsername(string username)
        {
            User user = new User() { Username = username };

            SqlCommand command = new SqlCommand(
                "SELECT id, description, url_image_profil FROM users WHERE username = @username", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar) { Value = username });
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                user.Id = reader.GetInt32(0);
                if(!reader.IsDBNull(1))
                    user.Description = reader.GetString(1);
                if(!reader.IsDBNull(2))
                    user.UrlImgProfil = reader.GetString(2);
            }

            command.Dispose();
            reader.Close();
            Connection.Instance.Close();
            return user;
        }

        public User GetUserById(int id)
        {
            User user = new User();
            SqlCommand command = new SqlCommand(
                "SELECT username, url_image_profil FROM users WHERE id = @id", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                user.Username = reader.GetString(0);
                if(!reader.IsDBNull(1))
                    user.UrlImgProfil = reader.GetString(1);
            }

            command.Dispose();
            reader.Close();
            Connection.Instance.Close();
            return user;
        }

        public User GetUserSubscriber(User user)
        {
            SqlCommand command = new SqlCommand(
                "SELECT * FROM abonnement WHERE id_abonnement = @id ", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = user.Id });
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<int> subscriberIds = new List<int>();
            while (reader.Read())
            {
                subscriberIds.Add(reader.GetInt32(1));
            }

            command.Dispose();
            reader.Close();
            Connection.Instance.Close();

            foreach(int i in subscriberIds)
            {
                user.Subcribers.Add(GetUserById(i));
            }
            return user;
        }

        public User GetUserSubscriptions(User user)
        {
            SqlCommand command = new SqlCommand(
                "SELECT * FROM abonnement WHERE id_abonne = @id ", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = user.Id });
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<int> subscriptionIds = new List<int>();
            while (reader.Read())
            {
                subscriptionIds.Add(reader.GetInt32(2));
            }

            command.Dispose();
            reader.Close();
            Connection.Instance.Close();
            foreach(int i in subscriptionIds)
            {
                user.Subscriptions.Add(GetUserById(i));
            }
            return user;
        }

        public User GetUserPost(User user)
        {
            SqlCommand command = new SqlCommand(
                "SELECT * FROM post WHERE id_user = @id ", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = user.Id });
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Post p = new Post();
                p.Id = reader.GetInt32(0);
                p.Description = reader.GetString(1);
                p.Date = reader.GetDateTime(2);
                user.Posts.Add(p);
            }

            command.Dispose();
            reader.Close();
            Connection.Instance.Close();

            foreach (Post p in user.Posts)
            {
                List<User> likes = new List<User>();
                List<Comment> comments = new List<Comment>();
                List<string> imgs = new List<string>();

                command = new SqlCommand("SELECT * FROM post_like WHERE id_post = @id", Connection.Instance);
                command.Parameters.Add(new SqlParameter("@id", p.Id));
                Connection.Instance.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (!reader.IsDBNull(2))
                    {
                        likes.Add(new User { Id = reader.GetInt32(2) });
                    }
                }

                reader.Close();
                command.Dispose();
                Connection.Instance.Close();

                command = new SqlCommand("SELECT * FROM comment WHERE id_post = @id", Connection.Instance);
                command.Parameters.Add(new SqlParameter("@id", p.Id));
                Connection.Instance.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        Comment c = new Comment()
                        {
                            Id = reader.GetInt32(0),
                            User = new User { Id = reader.GetInt32(1) },
                            Description = reader.GetString(3),
                            Date = reader.GetDateTime(4)
                        };
                        comments.Add(c);
                    }
                }

                reader.Close();
                command.Dispose();
                Connection.Instance.Close();

                command = new SqlCommand("SELECT * FROM post_img WHERE id_post = @id", Connection.Instance);
                command.Parameters.Add(new SqlParameter("@id", p.Id));
                Connection.Instance.Open();

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    imgs.Add(reader.GetString(2));
                }

                Connection.Instance.Close();

                p.Likes = likes;
                p.Comments = comments;
                p.Imgs = imgs;
            }

            return user;
        }

        public void Subscribe(Subscribe s)
        {
            SqlCommand command = new SqlCommand(
                "INSERT INTO abonnement (id_abonne, id_abonnement) VALUES (@idAbonne, @idAbonnement)", 
                Connection.Instance);
            command.Parameters.Add(new SqlParameter("@idAbonne", SqlDbType.Int) { Value = s.IdSubscriber });
            command.Parameters.Add(new SqlParameter("@idAbonnement", SqlDbType.Int) { Value = s.IdSubscription });
            Connection.Instance.Open();

            command.ExecuteNonQuery();
            command.Dispose();

            Connection.Instance.Close();
        }

        public void Unsubscribe(Subscribe s)
        {
            SqlCommand command = new SqlCommand(
                "DELETE FROM abonnement WHERE id_abonne = @idAbonne AND id_abonnement = @idAbonnement", 
                Connection.Instance);
            command.Parameters.Add(new SqlParameter("@idAbonne", SqlDbType.Int) { Value = s.IdSubscriber });
            command.Parameters.Add(new SqlParameter("@idAbonnement", SqlDbType.Int) { Value = s.IdSubscription });
            Connection.Instance.Open();

            command.ExecuteNonQuery();
            command.Dispose();

            Connection.Instance.Close();
        }

        public bool VerifSubscribed(Subscribe s)
        {
            bool subscribed = false;
            SqlCommand command = new SqlCommand(
                "SELECT * FROM abonnement WHERE id_abonne = @idAbonne AND id_abonnement = @idAbonnement", 
                Connection.Instance);
            command.Parameters.Add(new SqlParameter("@idAbonne", SqlDbType.Int) { Value = s.IdSubscriber });
            command.Parameters.Add(new SqlParameter("@idAbonnement", SqlDbType.Int) { Value = s.IdSubscription });
            Connection.Instance.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                subscribed = true;
                break;
            }

            command.Dispose();
            reader.Close();
            Connection.Instance.Close();

            return subscribed;
        }

        public void UploadProfilImg(string url, int id) {
            SqlCommand command = new SqlCommand(
                "UPDATE users SET url_image_profil = @url WHERE id = @id", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@url", url));
            command.Parameters.Add(new SqlParameter("@id", id));
            Connection.Instance.Open();

            command.ExecuteNonQuery();

            command.Dispose();
            Connection.Instance.Close();
        }

        public void UploadPost(Post p, List<IFormFile> files)
        {
            SqlCommand command = new SqlCommand(
                "INSERT INTO post (description, date, id_user) OUTPUT INSERTED.ID VALUES (@de,@da,@id)", 
                Connection.Instance);
            command.Parameters.Add(new SqlParameter("@de", p.Description));
            command.Parameters.Add(new SqlParameter("@da", p.Date));
            command.Parameters.Add(new SqlParameter("@id", p.IdUser));
            Connection.Instance.Open();
            int id = (int)command.ExecuteScalar();
            command.Dispose();
            Connection.Instance.Close();

            foreach(IFormFile file in files)
            {
                Task.Run(() => UploadPostImg(new PostUploadModelAndView()
                {
                    File = file,
                    IdPost = id
                }));
                //new Thread(UploadPostImg).Start(new PostUploadModelAndView()
                //{
                //    File = file,
                //    IdPost = id
                //});
            }
        }

        public void UploadPostImg(object o)
        {
            uploadPostMutex.WaitOne();
            PostUploadModelAndView p = o as PostUploadModelAndView;
            if (p.File != null && p.File.Length != 0)
            {
                string fileName = $"{p.IdPost}-{p.File.FileName}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
                var stream = new FileStream(path, FileMode.Create);
                p.File.CopyToAsync(stream);

                SqlCommand command = new SqlCommand(
                    "INSERT INTO post_img (id_post, url_image) VALUES (@i,@u)", Connection.Instance);
                command.Parameters.Add(new SqlParameter("@i", p.IdPost));
                command.Parameters.Add(new SqlParameter("@u", $"http://localhost:50255/api/user/img/{fileName}"));
                Connection.Instance.Open();

                command.ExecuteNonQuery();
                command.Dispose();

                Connection.Instance.Close();
            }

            uploadPostMutex.ReleaseMutex();
        }
    }
}
