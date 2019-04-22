using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SocialASPNET.Database
{
    public class Connection
    {
        private static SqlConnection _instance = null;
        private static object _lock = new object();
        public static SqlConnection Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Andrew\source\repos\Social\social.mdf;Integrated Security=True;Connect Timeout=30");
                    }
                    return _instance;
                }
            }
        }
    }
}
