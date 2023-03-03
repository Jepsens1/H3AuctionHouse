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
        protected string ConnectionString { get; }

        public DatabaseHelper(string connectionString) 
        { 
            ConnectionString= connectionString;
            
        }


        public async Task<UserModel> GetUser(string username)
        {
            SqlConnection SqlConnect = new SqlConnection(ConnectionString);
            await SqlConnect.OpenAsync();
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
                await SqlConnect.CloseAsync();
                return user;
            }
            await SqlConnect.CloseAsync();
            return null;
        }

        public async Task<UserModel>? GetUser(int id)
        {
            SqlConnection SqlConnect = new SqlConnection(ConnectionString);
            await SqlConnect.OpenAsync();
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
                await SqlConnect.CloseAsync();
                return user;
            }
            await SqlConnect.CloseAsync();
            return null;
        }
    }
}
