using AuctionHouseBackend.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Database
{
    /// <summary>
    /// Database help class
    /// Other database object can inherit from this object for easy database call access
    /// </summary>
    public class DatabaseHelper
    {
        public string ConnectionString { get; }
        public SqlConnection SqlConnect { get; protected set; }
        public SqlCommand SqlDataCommand { get; protected set; }
        public SqlDataReader SqlDataReader { get; protected set; }
        public SqlDataAdapter SqlDataAdapter { get; protected set; }

        public DatabaseHelper(string connectionString) 
        { 
            ConnectionString= connectionString;
            SqlConnect = new SqlConnection(ConnectionString);
        }

        protected void OpenConnection()
        {
            try
            {
                SqlConnect.Open();
            }
            catch { }
        }

        protected void CloseConnection()
        {
            try
            { 
                SqlConnect.Close(); 
            } catch { }
        }

        //protected int GetPrimaryTableId(string tableName, string idName)
        //{
        //    OpenConnection();
        //    string query = $"SELECT * FROM {tableName}";
        //    SqlDataCommand = new SqlCommand(query, SqlConnect);
        //    SqlDataReader = SqlDataCommand.ExecuteReader();
        //    int id = -1;
        //    if (SqlDataReader.Read())
        //    {
        //        id = Convert.ToInt32(SqlDataReader[idName]);
        //        CloseConnection();
        //        return id;
        //    }
        //    CloseConnection();
        //    return id;
        //}

        public UserModel GetUser(string username)
        {
            OpenConnection();
            string query = $"SELECT * FROM Users WHERE username = @username";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@username", username);
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

        public UserModel? GetUser(int id)
        {
            OpenConnection();
            string query = $"SELECT * FROM Users WHERE id = @id";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@id", id);
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
