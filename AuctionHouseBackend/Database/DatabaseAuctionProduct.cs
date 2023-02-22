using AuctionHouseBackend.Interfaces;
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
    public class DatabaseAuctionProduct : DatabaseHandler
    {
        public DatabaseAuctionProduct(string connectionString) : base(connectionString)
        {
        }

        public List<AuctionProductModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Create(AuctionProductModel product)
        {
            try
            {
                OpenConnection();
                string query = $"INSERT INTO Product(userId) VALUES ({product.Owner.Id})";
                SqlDataCommand = new SqlCommand(query, SqlConnect);
                SqlDataCommand.ExecuteScalar();
                CloseConnection();
                OpenConnection();
                string query2 = $"INSERT INTO AuctionProducts(productId, productName, productDescription, productStatus, productCategory, expireryTime, expireryDate) VALUES " +
                    $"({product.Owner.Id}, '{product.Name}', '{product.Description}', {(int)product.Status}, {(int)product.Category}, {product.ExpireryTime.ToShortDateString()}, {product.ExpireryDate.ToShortDateString()})";
                SqlDataCommand = new SqlCommand(query2, SqlConnect);
                SqlDataCommand.ExecuteScalar();
                CloseConnection();
            }
            catch (Exception ex)
            {
                CloseConnection();
                return false;
            }
            return true;
        }

        public List<AuctionProductModel> GetUserProducts(int userId)
        {
            //int productId = GetPrimaryTableId("Product", "userId");
            UserModel owner = GetUser(userId);
            OpenConnection();
            string query = $"SELECT * FROM AuctionProducts WHERE productId = {owner.Id}";
            SqlDataCommand = new SqlCommand(query, SqlConnect);
            SqlDataReader = SqlDataCommand.ExecuteReader();
            List<AuctionProductModel> products = new List<AuctionProductModel>();
            while (SqlDataReader.Read())
            {
                int id = Convert.ToInt32(SqlDataReader["productId"]);
                string name = SqlDataReader["productName"].ToString();
                string description = SqlDataReader["productDescription"].ToString();
                Category category = (Category)SqlDataReader["productCategory"];
                Status status = (Status)SqlDataReader["productStatus"];
                DateTime expTime = (DateTime)SqlDataReader["expireryTime"];
                DateTime expDate = (DateTime)SqlDataReader["expireryDate"];
                int highestBidderId = 0;
                decimal price = 0;
                if (!DBNull.Value.Equals(SqlDataReader["highestBidderId"]))
                {
                    highestBidderId = Convert.ToInt32(SqlDataReader["highestBidderId"]);
                    price = Convert.ToDecimal(SqlDataReader["price"]);
                }
                UserModel user = new UserModel(highestBidderId);
                AuctionBidderModel bidder = new AuctionBidderModel(user, price);
                AuctionProductModel model = new AuctionProductModel(id, name, description, category, status, expTime, expDate, owner);
                if (bidder != null )
                {
                    model.HighestBidder = bidder;
                }
                products.Add(model);
            }
            CloseConnection();

            for (int i = 0; i < products.Count; i++)
            {
                UserModel user = GetUser(products[i].Id);
                products[i].HighestBidder.User = user;
            }

            return products;
        }

        public AuctionProductModel GetProduct(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
