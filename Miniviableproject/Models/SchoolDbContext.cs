using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;

namespace Miniviableproject.Models
{
    public class SchoolDbContext
    {

        private static string User { get { return "root"; } }
        private static string Password { get { return ""; } }
        private static string Database { get { return "schooldb"; } }
        private static string Server { get { return "localhost"; } }
        private static string Port { get { return "3306"; } }

        protected static string ConnectionString
        {
            get
            {
                return "server = " + Server
                    + "; user = " + User
                    + "; database = " + Database
                    + "; port = " + Port
                    + "; password = " + Password
                    + ";convert zero datetime=True;sslmode=None";
            }
        }

        public MySqlConnection AccessDatabase()
        {

            return new MySqlConnection(ConnectionString);
        }
        public DbSet<Teacher> Teachers { get; set; }

    }
}