using AuctionHouseBackend.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            string query = $"INSERT INTO ProductImages(productId, images) VALUES(@productId, @img)";
            SqlCommand SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@productId", productId);
            var byteParam = new SqlParameter("@img", SqlDbType.VarBinary)
            {
                Direction = ParameterDirection.Input,
                Size = img.Length,
                Value = img
            };
            SqlDataCommand.Parameters.Add(byteParam);
            await SqlDataCommand.ExecuteReaderAsync();
            await SqlConnect.CloseAsync();
        }

        public async Task<List<byte[]>> GetImages(int productId)
        {
            List<byte[]> images = new List<byte[]>();
            SqlConnection SqlConnect = new SqlConnection(ConnectionString);
            await SqlConnect.OpenAsync();
            string query = $"SELECT * FROM ProductImages WHERE productId = @productId";
            SqlCommand SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@productId", productId);
            SqlDataReader SqlDataReader = await SqlDataCommand.ExecuteReaderAsync();
            while (SqlDataReader.Read())
            {
                images.Add((byte[])SqlDataReader["images"]);
            }
            await SqlConnect.CloseAsync();
            return images;
        }
    }
}
