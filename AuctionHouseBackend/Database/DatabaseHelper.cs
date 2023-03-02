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
        //public SqlConnection SqlConnect { get; protected set; }
        //public SqlCommand SqlDataCommand { get; protected set; }
        //public SqlDataReader SqlDataReader { get; protected set; }
        //public SqlDataAdapter SqlDataAdapter { get; protected set; }

        public DatabaseHelper(string connectionString) 
        { 
            ConnectionString= connectionString;
            
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

        public async Task<UserModel> GetUser(string username)
        {
            SqlConnection SqlConnect = new SqlConnection(ConnectionString);
            SqlConnect.Open();
            string query = $"SELECT * FROM Users WHERE username = @username";
            SqlCommand SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@username", username);
            SqlDataReader SqlDataReader = await SqlDataCommand.ExecuteReaderAsync();
            UserModel user;
            if (SqlDataReader.Read())
            {
                user = new UserModel(SqlDataReader["firstName"].ToString(), SqlDataReader["lastName"].ToString(),
                    SqlDataReader["username"].ToString(), SqlDataReader["email"].ToString(), "");
                user.Id = Convert.ToInt32(SqlDataReader["id"]);
                SqlConnect.Close();
                return user;
            }
            SqlConnect.Close();
            return null;
        }

        public async Task<UserModel>? GetUser(int id)
        {
            SqlConnection SqlConnect = new SqlConnection(ConnectionString);
            SqlConnect.Open();
            string query = $"SELECT * FROM Users WHERE id = @id";
            SqlCommand SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@id", id);
            SqlDataReader SqlDataReader = await SqlDataCommand.ExecuteReaderAsync();
            UserModel user;
            if (SqlDataReader.Read())
            {
                user = new UserModel(SqlDataReader["firstName"].ToString(), SqlDataReader["lastName"].ToString(),
                    SqlDataReader["username"].ToString(), SqlDataReader["email"].ToString(), "");
                user.Id = Convert.ToInt32(SqlDataReader["id"]);
                SqlConnect.Close();
                return user;
            }
            SqlConnect.Close();
            return null;
        }
    }
}
