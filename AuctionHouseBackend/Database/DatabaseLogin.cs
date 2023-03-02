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
        public async Task<UserModel>? Login(string username)
        {
            UserModel user = await GetUser(username);
            if (user != null)
            {
                user.Hash = await GetHash(user.Id);
                return user;
            }
            return null;
        }

        /// <summary>
        /// Creates and account
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> CreateAccount(UserModel user)
        {
            UserModel userModel = await GetUser(user.Username);
            SqlConnection SqlConnect = new SqlConnection(ConnectionString);
            await SqlConnect.OpenAsync();
            if (userModel == null)
            {
                string query = $"INSERT INTO Users(username, firstName, lastName, email) VALUES(@username, @firstName, " +
                    $"@lastName, @email)";
                SqlCommand SqlDataCommand = new SqlCommand(query, SqlConnect);
                SqlDataCommand.Parameters.AddWithValue("@username", user.Username);
                SqlDataCommand.Parameters.AddWithValue("@firstName", user.FirstName);
                SqlDataCommand.Parameters.AddWithValue("@lastName", user.LastName);
                SqlDataCommand.Parameters.AddWithValue("@email", user.Email);
                await SqlDataCommand.ExecuteScalarAsync();
                await SqlConnect.CloseAsync();
                await CreateSalt(user);
                return true;
            }
            await SqlConnect.CloseAsync();
            return false;
        }

        public async Task UpdateLogin(int id, HashModel hash)
        {
            SqlConnection SqlConnect = new SqlConnection(ConnectionString);
            await SqlConnect.OpenAsync();
            string query = $"UPDATE Hashes SET hash = @hash, salt = @salt WHERE id = @id";
            SqlCommand SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@hash", hash.Hash);
            SqlDataCommand.Parameters.AddWithValue("@salt", hash.Salt);
            SqlDataCommand.Parameters.AddWithValue("@id", id);
            await SqlDataCommand.ExecuteScalarAsync();
            await SqlConnect.CloseAsync();
        }

        private async Task<HashModel>? GetHash(int id)
        {
            SqlConnection SqlConnect = new SqlConnection(ConnectionString);
            await SqlConnect.OpenAsync();
            string query = $"SELECT * FROM Hashes WHERE id = @id";
            SqlCommand SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@id", id);
            SqlDataReader SqlDataReader = await SqlDataCommand.ExecuteReaderAsync();
            if (SqlDataReader.Read())
            {
                HashModel hash = new HashModel(SqlDataReader["hash"].ToString(), SqlDataReader["Salt"].ToString());
                await SqlConnect.CloseAsync();
                return hash;
            }
            await SqlConnect.CloseAsync();
            return null;
        }

        private async Task CreateSalt(UserModel user)
        {
            int id = GetUser(user.Username).Id;
            SqlConnection SqlConnect = new SqlConnection(ConnectionString);
            await SqlConnect.OpenAsync();
            string query = $"INSERT INTO Hashes(id, hash, salt) VALUES(@id, @hash, @salt)";
            SqlCommand SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@hash", user.Hash.Hash);
            SqlDataCommand.Parameters.AddWithValue("@salt", user.Hash.Salt);
            SqlDataCommand.Parameters.AddWithValue("@id", id);
            await SqlDataCommand.ExecuteScalarAsync();
            await SqlConnect.CloseAsync();
        }
    }
}
