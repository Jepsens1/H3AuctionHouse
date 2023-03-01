using AuctionHouseBackend.Interfaces;
using AuctionHouseBackend.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Database
{
    /// <summary>
    /// This class handles the auction product database calls
    /// Every database call should have a try/catch but some i haven't added yet
    /// </summary>
    public class DatabaseAuctionProduct : DatabaseHelper
    {
        public DatabaseAuctionProduct(string connectionString) : base(connectionString)
        {
        }

        private ProductModel<AuctionProductModel> GetProductHelper()
        {
            int id = Convert.ToInt32(SqlDataReader["productId"]);
            string name = SqlDataReader["productName"].ToString();
            string description = SqlDataReader["productDescription"].ToString();
            Category category = (Category)SqlDataReader["productCategory"];
            Status status = (Status)SqlDataReader["productStatus"];
            DateTime expDate = DateTime.Parse(SqlDataReader["expireryDate"].ToString());
            int highestBidderId = 0;
            decimal price = 0;
            if (!DBNull.Value.Equals(SqlDataReader["highestBidderId"]))
            {
                highestBidderId = Convert.ToInt32(SqlDataReader["highestBidderId"]);
                price = Convert.ToDecimal(SqlDataReader["price"]);
            }
            UserModel user = new UserModel(highestBidderId);
            AuctionBidderModel bidder = new AuctionBidderModel(user, price);
            AuctionProductModel model = new AuctionProductModel(id, name, description, category, status, expDate);
            ProductModel<AuctionProductModel> productModel = new ProductModel<AuctionProductModel>(model, null);
            if (bidder != null)
            {
                model.HighestBidder = bidder;
            }
            return productModel;
        }

        public async Task<List<ProductModel<AuctionProductModel>>> GetAll()
        {
            List<ProductModel<AuctionProductModel>> products = new List<ProductModel<AuctionProductModel>>();
            try
            {

                OpenConnection();
                string query = $"SELECT * FROM AuctionProducts";
                SqlDataCommand = new SqlCommand(query, SqlConnect);
                SqlDataReader = await SqlDataCommand.ExecuteReaderAsync();
                while (SqlDataReader.Read())
                {
                    products.Add(GetProductHelper());
                }
                CloseConnection();
                for (int i = 0; i < products.Count; i++)
                {
                    products[i].Owner = await GetUserFromProductId(products[i].Product.Id);
                }
            }
            catch (Exception ex)
            {
                CloseConnection();
                Logger.AddLog(LogLevel.ERROR, "AddAll() " + ex.Message);
                return new List<ProductModel<AuctionProductModel>>();
            }
            return products;
        }

        public async Task<ProductModel<AuctionProductModel>>? GetProduct(int productId)
        {
            try
            {
                UserModel owner = await GetUserFromProductId(productId);
                OpenConnection();
                string query = $"SELECT * FROM AuctionProducts WHERE productId = @id";
                SqlDataCommand = new SqlCommand(query, SqlConnect);
                SqlDataCommand.Parameters.AddWithValue("@id", productId);
                SqlDataReader = await SqlDataCommand.ExecuteReaderAsync();
                ProductModel<AuctionProductModel>? product = null;
                if (SqlDataReader.Read())
                {
                    product = GetProductHelper();
                }
                CloseConnection();
                if (product != null)
                {
                    UserModel user = await GetUser(product.Product.Id);
                    product.Product.HighestBidder.User = user;
                    product.Owner = await GetUserFromProductId(productId);
                    return product;
                }
                return null;
            }
            catch (Exception ex)
            {
                CloseConnection();
                Logger.AddLog(LogLevel.ERROR, "GetProduct() " + ex.Message);
                return null;
            }
        }

        public async Task<bool> Create(ProductModel<AuctionProductModel> product)
        {
            try
            {
                OpenConnection();
                DateTime date = product.Product.ExpireryDate;
                string sqlFormattedDate = date.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string query = $"INSERT INTO Product(userId) VALUES (@ownerId)";
                SqlDataCommand = new SqlCommand(query, SqlConnect);
                SqlDataCommand.Parameters.AddWithValue("@ownerId", product.Owner.Id);
                await SqlDataCommand.ExecuteScalarAsync();
                string query2 = $"SELECT TOP 1 * FROM Product ORDER BY id DESC ";
                SqlDataCommand.CommandText = query2;
                int productId = Convert.ToInt32(await SqlDataCommand.ExecuteScalarAsync());
                string query3 = $"INSERT INTO AuctionProducts(productId, productName, productDescription, productStatus, productCategory, expireryDate) VALUES (@productId, @productName," +
                    $" @productDescription, @productStatus, @productCategory, @productExpireDate)";
                SqlDataCommand.CommandText = query3;
                SqlDataCommand.Parameters.AddWithValue("@productId", productId);
                SqlDataCommand.Parameters.AddWithValue("@productName", product.Product.Name);
                SqlDataCommand.Parameters.AddWithValue("@productDescription", product.Product.Description);
                SqlDataCommand.Parameters.AddWithValue("@productStatus", (int)product.Product.Status);
                SqlDataCommand.Parameters.AddWithValue("@productCategory", (int)product.Product.Category);
                SqlDataCommand.Parameters.AddWithValue("@productExpireDate", sqlFormattedDate);
                await SqlDataCommand.ExecuteNonQueryAsync();
                CloseConnection();

            }
            catch (Exception ex)
            {
                CloseConnection();
                Logger.AddLog(LogLevel.ERROR, "Create() " + ex.Message);
                return false;
            }
            return true;
        }

        public async Task<List<ProductModel<AuctionProductModel>>> GetUserProducts(int userId)
        {
            int productId = await GetProductIdFromUserId(userId);
            try
            {
                OpenConnection();
                string query = $"SELECT * FROM AuctionProducts WHERE productId = @id";
                SqlDataCommand = new SqlCommand(query, SqlConnect);
                SqlDataCommand.Parameters.AddWithValue("@id", productId);
                SqlDataReader = await SqlDataCommand.ExecuteReaderAsync();
                List<ProductModel<AuctionProductModel>> products = new List<ProductModel<AuctionProductModel>>();
                while (SqlDataReader.Read())
                {
                    products.Add(GetProductHelper());
                }
                CloseConnection();

                for (int i = 0; i < products.Count; i++)
                {
                    UserModel user = await GetUserFromProductId(products[i].Product.HighestBidder.User.Id);
                    products[i].Product.HighestBidder.User = user;
                }
                return products;
            }
            catch (Exception ex)
            {
                CloseConnection();
                Logger.AddLog(LogLevel.ERROR, "GetUserProducts() " + ex.Message);
                return new List<ProductModel<AuctionProductModel>>();
            }
        }

        public async void SetHighestBidder(int userId, int productId, decimal price)
        {
            OpenConnection();
            string query = $"UPDATE AuctionProducts SET highestBidderId = @userId, price = @price WHERE productId = @productId";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@userId", userId);
            SqlDataCommand.Parameters.AddWithValue("@price", price);
            SqlDataCommand.Parameters.AddWithValue("@productId", productId);
            SqlDataReader = await SqlDataCommand.ExecuteReaderAsync();
            CloseConnection();
        }

        public async void UpdateExpireryDate(int productId, DateTime expireTime)
        {
            OpenConnection();
            string query = $"UPDATE AuctionProducts SET expireryDate = @expireryDate WHERE productId = @productId";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            string sqlFormattedDate = expireTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            SqlDataCommand.Parameters.AddWithValue("@expireryDate", sqlFormattedDate);
            SqlDataCommand.Parameters.AddWithValue("@productId", productId);
            SqlDataReader = await SqlDataCommand.ExecuteReaderAsync();
            CloseConnection();
        }

        public async void AddAutobid(int userId, int productId, decimal autobidPrice, decimal maximumPrice)
        {
            OpenConnection();
            string query = $"INSERT INTO Bids(productId, userId, autobidPrice, maximumPrice) VALUES(@productId, @userId, @autobidPrice, @maximumPrice)";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@productId", productId);
            SqlDataCommand.Parameters.AddWithValue("@userId", userId);
            SqlDataCommand.Parameters.AddWithValue("@autobidPrice", autobidPrice);
            SqlDataCommand.Parameters.AddWithValue("@maximumPrice", maximumPrice);
            SqlDataReader = await SqlDataCommand.ExecuteReaderAsync();
            CloseConnection();
        }

        public async Task<List<AutobidModel>> GetAutobids()
        {
            OpenConnection();
            string query = $"SELECT * FROM Bids";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataReader = await SqlDataCommand.ExecuteReaderAsync();
            List<AutobidModel> models = new List<AutobidModel>();
            while (SqlDataReader.Read())
            {
                models.Add(new AutobidModel(Convert.ToInt32(SqlDataReader["userId"]), Convert.ToInt32(SqlDataReader["productId"]), Convert.ToDecimal(SqlDataReader["autobidPrice"]), Convert.ToDecimal(SqlDataReader["maximumPrice"])));
            }
            CloseConnection();
            return models;
        }

        public async void UpdateAutoBid(int userId, int productId, decimal autobidPrice, decimal maximumPrice)
        {
            OpenConnection();
            string query = $"UPDATE Bids SET maximumPrice = @maximumPrice AND autobidPrice = @autobidPrice WHERE productId = @productId AND userId = @userId";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@productId", productId);
            SqlDataCommand.Parameters.AddWithValue("@userId", userId);
            SqlDataCommand.Parameters.AddWithValue("@autobidPrice", autobidPrice);
            SqlDataCommand.Parameters.AddWithValue("@maximumPrice", maximumPrice);
            SqlDataReader = await SqlDataCommand.ExecuteReaderAsync();
            CloseConnection();
        }



        public async void ChangeStatus(int productId, Status status)
        {
            OpenConnection();
            string query = $"UPDATE AuctionProducts SET productStatus = @status WHERE productId = @id";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@status", status);
            SqlDataCommand.Parameters.AddWithValue("@id", productId);
            SqlDataReader = await SqlDataCommand.ExecuteReaderAsync();
            CloseConnection();
        }

        private async Task<UserModel> GetUserFromProductId(int id)
        {
            OpenConnection();
            int userId = 0;
            string query = $"SELECT userId FROM Product WHERE id = @id";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@id", id);
            SqlDataReader = await SqlDataCommand.ExecuteReaderAsync();
            if (SqlDataReader.Read())
            {
                userId = Convert.ToInt32(SqlDataReader["userId"]);
            }
            CloseConnection();
            return await GetUser(userId);
        }

        private async Task<int> GetProductIdFromUserId(int userId)
        {
            OpenConnection();
            int id = 0;
            string query = $"SELECT id FROM Product WHERE userId = @id";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataCommand.Parameters.AddWithValue("@id", userId);
            SqlDataReader = await SqlDataCommand.ExecuteReaderAsync();
            if (SqlDataReader.Read())
            {
                id = Convert.ToInt32(SqlDataReader["id"]);
            }
            CloseConnection();
            return id;
        }
    }
}
