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
            SqlCommand command = new SqlCommand("SELECT id FROM users WHERE username=@username AND password=@password", 
                Connection.Instance);
            command.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar) { Value = user.Username });
            command.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar) { Value = user.Password});
            Connection.Instance.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                user.Id = reader.GetInt32(0);
            }

            command.Dispose();
            reader.Close();
            Connection.Instance.Close();
            return user;
        }
    }
}
