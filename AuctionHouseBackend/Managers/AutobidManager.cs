using AuctionHouseBackend.Database;
using AuctionHouseBackend.Interfaces;
using AuctionHouseBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Managers
{
    public class AutobidManager
    {
        public List<AutobidModel> AutobidModels { get; set; }
        private DatabaseAuctionProduct databaseAuctionProduct;

        public AutobidManager(DatabaseAuctionProduct databaseAuctionProduct)
        {
            this.databaseAuctionProduct = databaseAuctionProduct;
            AutobidModels = databaseAuctionProduct.GetAutobids();
        }

        public void AddAutobid(int userId, int productId, decimal price, decimal autobidPrice, decimal maximumPrice)
        {
            ProductModel<AuctionProductModel> product = databaseAuctionProduct.GetProduct(productId);
            if (product.Product.HighestBidder.Price < price)
            {
                databaseAuctionProduct.AddAutobid(userId, productId, price, autobidPrice, maximumPrice);
            }
        }

        public void Autobid(AuctionProductManager productManager, ProductModel<AuctionProductModel> product)
        {
            if (AutobidModels.Count <= 0)
            {
                return;
            }
            AutobidModel highestBidder = AutobidModels[0];
            for (int i = 0; i < AutobidModels.Count; i++)
            {
                if (highestBidder.AutobidPrice < AutobidModels[i].AutobidPrice && highestBidder.AutobidMax >= product.Product.HighestBidder.Price)
                {
                    highestBidder = AutobidModels[i];
                }
            }
            if (product.Product.HighestBidder.User.Id == highestBidder.UserId)
            {
                return;
            }
            decimal finalPrice = highestBidder.AutobidPrice + product.Product.HighestBidder.Price;
            productManager.BidOnProduct(highestBidder.UserId, product, finalPrice);
        }
    }
}
