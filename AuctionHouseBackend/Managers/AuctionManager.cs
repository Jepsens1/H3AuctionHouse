using AuctionHouseBackend.Database;
using AuctionHouseBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Managers
{
    public class AuctionManager
    {

        private DatabaseAuctionProduct auctionProduct;

        public AuctionManager(DatabaseAuctionProduct auctionProduct)
        {
            this.auctionProduct = auctionProduct;
        }

        public ErrorCodes BidOnProduct(int userId, int productId, decimal amount)
        {
            ProductModel<AuctionProductModel> product = auctionProduct.GetProduct(productId);
            if (product.Product.HighestBidder.Price >= amount)
            {
                return ErrorCodes.BidTooLow;
            }
            return ErrorCodes.NoError;
        }


    }
}
