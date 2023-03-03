using AuctionHouseBackend.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Database
{
    public class DatabaseProduct : DatabaseHelper
    {
        public DatabaseProduct(string connectionString) : base(connectionString)
        {
        }

        public async Task InsertImg(byte[] img, int productId)
        {
            SqlConnection SqlConnect = new SqlConnection(ConnectionString);
            await SqlConnect.OpenAsync();
            string query = $"INSERT INTO ProductImages(productId, images) VALUES(@productId, (SELECT * FROM OPENROWSET(BULK @img, SINGLE_BLOB) as image))";
            SqlCommand SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@productId", productId);
            SqlDataCommand.Parameters.AddWithValue("@img", img);
            await SqlDataCommand.ExecuteReaderAsync();
            await SqlConnect.CloseAsync();
        }

        public async Task<List<string>> GetImages(int productId)
        {
            List<string> images = new List<string>();
            SqlConnection SqlConnect = new SqlConnection(ConnectionString);
            await SqlConnect.OpenAsync();
            string query = $"SELECT * FROM ProductImages WHERE productId = @productId";
            SqlCommand SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@productId", productId);
            SqlDataReader SqlDataReader = await SqlDataCommand.ExecuteReaderAsync();
            while (SqlDataReader.Read())
            {
                images.Add(SqlDataReader["images"].ToString());
            }
            await SqlConnect.CloseAsync();
            return images;
        }
    }
}
