using SocialASPNET.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SocialASPNET.Database
{
    public class UserDatabase
    {
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
                "SELECT username, url_image FROM users WHERE id = @id", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                user.Username = reader.GetString(0);
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

            while (reader.Read())
            {
                user.Subcribers.Add(GetUserById(reader.GetInt32(0)));
            }

            command.Dispose();
            reader.Close();
            Connection.Instance.Close();
            return user;
        }

        public User GetUserSubscriptions(User user)
        {
            SqlCommand command = new SqlCommand(
                "SELECT * FROM abonnement WHERE id_abonne = @id ", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = user.Id });
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                user.Subscriptions.Add(GetUserById(reader.GetInt32(1)));
            }

            command.Dispose();
            reader.Close();
            Connection.Instance.Close();
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
                p.Description = reader.GetString(1);
                p.Date = reader.GetDateTime(2);
                user.Posts.Add(p);
            }

            command.Dispose();
            reader.Close();
            Connection.Instance.Close();
            return user;
        }
    }
}
