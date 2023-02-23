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

        public List<ProductModel<AuctionProductModel>> GetAll()
        {
            List<ProductModel<AuctionProductModel>> products = new List<ProductModel<AuctionProductModel>>();
            try
            {

                OpenConnection();
                string query = $"SELECT * FROM AuctionProducts";
                SqlDataCommand = new SqlCommand(query, SqlConnect);
                SqlDataReader = SqlDataCommand.ExecuteReader();
                while (SqlDataReader.Read())
                {
                    products.Add(GetProductHelper());
                }
                CloseConnection();
                for (int i = 0; i < products.Count; i++)
                {
                    products[i].Owner = GetUserFromProductId(products[i].Product.Id);
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

        public ProductModel<AuctionProductModel> GetProduct(int productId)
        {
            try
            {
                UserModel owner = GetUserFromProductId(productId);
                OpenConnection();
                string query = $"SELECT * FROM AuctionProducts WHERE productId = {productId}";
                SqlDataCommand = new SqlCommand(query, SqlConnect);
                SqlDataReader = SqlDataCommand.ExecuteReader();
                ProductModel<AuctionProductModel> product = null;
                if (SqlDataReader.Read())
                {
                    product = GetProductHelper();
                }
                CloseConnection();
                UserModel user = GetUser(product.Product.Id);
                product.Product.HighestBidder.User = user;
                product.Owner = GetUserFromProductId(productId);
                return product;
            }
            catch (Exception ex)
            {
                CloseConnection();
                Logger.AddLog(LogLevel.ERROR, "GetProduct() " + ex.Message);
                return null;
            }
        }

        public bool Create(ProductModel<AuctionProductModel> product)
        {
            try
            {
                OpenConnection();
                DateTime date = product.Product.ExpireryDate;
                string sqlFormattedDate = date.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string query = $"INSERT INTO Product(userId) VALUES ({product.Owner.Id}) INSERT INTO AuctionProducts(productId, productName, productDescription, productStatus, productCategory, expireryDate) VALUES " +
                    $"({product.Owner.Id}, '{product.Product.Name}', '{product.Product.Description}', {(int)product.Product.Status}, {(int)product.Product.Category}, " +
                    $"'{sqlFormattedDate}')";
                SqlDataCommand = new SqlCommand(query, SqlConnect);
                SqlDataCommand.ExecuteScalar();
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

        public List<ProductModel<AuctionProductModel>> GetUserProducts(int userId)
        {
            int productId = GetProductIdFromUserId(userId);
            UserModel owner = GetUser(userId);
            try
            {
                OpenConnection();
                string query = $"SELECT * FROM AuctionProducts WHERE productId = {productId}";
                SqlDataCommand = new SqlCommand(query, SqlConnect);
                SqlDataReader = SqlDataCommand.ExecuteReader();
                List<ProductModel<AuctionProductModel>> products = new List<ProductModel<AuctionProductModel>>();
                while (SqlDataReader.Read())
                {
                    products.Add(GetProductHelper());
                }
                CloseConnection();

                for (int i = 0; i < products.Count; i++)
                {
                    UserModel user = GetUserFromProductId(products[i].Product.HighestBidder.User.Id);
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

        public void SetHighestBidder(int userId, int productId, decimal price)
        {
            OpenConnection();
            string query = $"UPDATE AuctionProducts SET highestBidderId = {userId}, price = {price} WHERE productId = {productId}";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataReader = SqlDataCommand.ExecuteReader();
            CloseConnection();
        }

        public void ChangeStatus(int productId, Status status)
        {
            OpenConnection();
            string query = $"UPDATE AuctionProducts SET productStatus = {(int)status} WHERE productId = {productId}";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataReader = SqlDataCommand.ExecuteReader();
            CloseConnection();
        }

        public ProductModel<AuctionProductModel> GetProduct(Category category)
        {
            throw new NotImplementedException();
        }

        private UserModel GetUserFromProductId(int id)
        {
            OpenConnection();
            int userId = 0;
            string query = $"SELECT userId FROM Product WHERE id = {id}";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataReader = SqlDataCommand.ExecuteReader();
            if (SqlDataReader.Read())
            {
                userId = Convert.ToInt32(SqlDataReader["userId"]);
            }
            CloseConnection();
            return GetUser(userId);
        }

        private int GetProductIdFromUserId(int userId)
        {
            OpenConnection();
            int id = 0;
            string query = $"SELECT id FROM Product WHERE userId = {userId}";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataReader = SqlDataCommand.ExecuteReader();
            if (SqlDataReader.Read())
            {
                id = Convert.ToInt32(SqlDataReader["id"]);
            }
            CloseConnection();
            return id;
        }
    }
}
