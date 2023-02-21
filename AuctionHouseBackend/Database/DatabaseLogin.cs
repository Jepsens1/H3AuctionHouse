using AuctionHouseBackend.Cryption;
using AuctionHouseBackend.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Database
{
    public class DatabaseLogin : DatabaseHandler
    {
        public DatabaseLogin(string connectionString) : base(connectionString)
        {
        }

        // 
        public UserModel Login(string username)
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
                string query = $"INSERT INTO Users(username, firstName, lastName, email) VALUES('{user.Username}', '{user.FirstName}', " +
                    $"'{user.LastName}', '{user.Email}')";
                SqlDataCommand = new SqlCommand(query, SqlConnect);
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
            string query = $"UPDATE Hashes SET hash = '{hash.Hash}', salt = '{hash.Salt}' WHERE id = {id}";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.ExecuteScalar();
            CloseConnection();
        }

        private HashModel GetHash(int id)
        {
            OpenConnection();
            string query = $"SELECT * FROM Hashes WHERE id = {id}";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataReader = SqlDataCommand.ExecuteReader();
            HashModel hash;
            if (SqlDataReader.Read())
            {
                hash = new HashModel(SqlDataReader["hash"].ToString(), SqlDataReader["Salt"].ToString());
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
            string query = $"INSERT INTO Hashes(id, hash, salt) VALUES({id}, '{user.Hash.Hash}', '{user.Hash.Salt}')";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.ExecuteScalar();
            CloseConnection();
        }
    }
}
