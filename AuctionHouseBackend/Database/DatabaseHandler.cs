using AuctionHouseBackend.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Database
{
    public class DatabaseHandler
    {
        public string ConnectionString { get; }
        public SqlConnection SqlConnect { get; protected set; }
        public SqlCommand SqlDataCommand { get; protected set; }
        public SqlDataReader SqlDataReader { get; protected set; }
        public SqlDataAdapter SqlDataAdapter { get; protected set; }

        public DatabaseHandler(string connectionString) 
        { 
            ConnectionString= connectionString;
            SqlConnect = new SqlConnection(ConnectionString);
        }

        protected void OpenConnection()
        {
            SqlConnect.Open();
        }

        protected void CloseConnection()
        {
            SqlConnect.Close();
        }

        protected UserModel GetUser(string username)
        {
            OpenConnection();
            string query = $"SELECT * FROM Users WHERE username = '{username}'";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataReader = SqlDataCommand.ExecuteReader();
            UserModel user;
            if (SqlDataReader.Read())
            {
                user = new UserModel(SqlDataReader["firstName"].ToString(), SqlDataReader["lastName"].ToString(),
                    SqlDataReader["username"].ToString(), SqlDataReader["email"].ToString(), "");
                user.Id = Convert.ToInt32(SqlDataReader["id"]);
                CloseConnection();
                return user;
            }
            CloseConnection();
            return null;
        }
    }
}
