using AuctionHouseBackend.Cryption;
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
    /// This object handles Login and registration
    /// </summary>
    public class DatabaseLogin : DatabaseHelper
    {
        public DatabaseLogin(string connectionString) : base(connectionString)
        {
        }

        // 
        public UserModel? Login(string username)
        {
            UserModel user = GetUser(username);
            if (user != null)
            {
                user.Hash = GetHash(user.Id);
                return user;
            }
            return null;
        }

        public bool CreateAccount(UserModel user)
        {
            UserModel userModel = GetUser(user.Username);
            OpenConnection();
            if (userModel == null)
            {
                string query = $"INSERT INTO Users(username, firstName, lastName, email) VALUES(@username, @firstName, " +
                    $"@lastName, @email)";
                SqlDataCommand = new SqlCommand(query, SqlConnect);
                SqlDataCommand.Parameters.AddWithValue("@username", user.Username);
                SqlDataCommand.Parameters.AddWithValue("@firstName", user.FirstName);
                SqlDataCommand.Parameters.AddWithValue("@lastName", user.LastName);
                SqlDataCommand.Parameters.AddWithValue("@email", user.Email);
                SqlDataCommand.ExecuteScalar();
                CloseConnection();
                CreateSalt(user);
                return true;
            }
            CloseConnection();
            return false;
        }

        public void UpdateLogin(int id, HashModel hash)
        {
            OpenConnection();
            string query = $"UPDATE Hashes SET hash = @hash, salt = @salt WHERE id = @id";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@hash", hash.Hash);
            SqlDataCommand.Parameters.AddWithValue("@salt", hash.Salt);
            SqlDataCommand.Parameters.AddWithValue("@id", id);
            SqlDataCommand.ExecuteScalar();
            CloseConnection();
        }

        private HashModel? GetHash(int id)
        {
            OpenConnection();
            string query = $"SELECT * FROM Hashes WHERE id = @id";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@id", id);
            SqlDataReader = SqlDataCommand.ExecuteReader();
            if (SqlDataReader.Read())
            {
                HashModel hash = new HashModel(SqlDataReader["hash"].ToString(), SqlDataReader["Salt"].ToString());
                CloseConnection();
                return hash;
            }
            CloseConnection();
            return null;
        }

        private void CreateSalt(UserModel user)
        {
            int id = GetUser(user.Username).Id;
            OpenConnection();
            string query = $"INSERT INTO Hashes(id, hash, salt) VALUES(@id, @hash, @salt)";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@hash", user.Hash.Hash);
            SqlDataCommand.Parameters.AddWithValue("@salt", user.Hash.Salt);
            SqlDataCommand.Parameters.AddWithValue("@id", id);
            SqlDataCommand.ExecuteScalar();
            CloseConnection();
        }
    }
}
