using AuctionHouseBackend.Database;
using AuctionHouseBackend.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Managers
{
    /// <summary>
    /// This object handles the logic for all the auctions
    /// </summary>
    public class AuctionManager
    {
        private DatabaseAuctionProduct auctionProduct;

        public AuctionManager(DatabaseAuctionProduct auctionProduct)
        {
            this.auctionProduct = auctionProduct;
            Thread t = new Thread(CheckForFinnishedAuctions);
            t.Start();
        }


        /// <summary>
        /// Function for the user to bid on products
        /// If user bids on product with 10 minutes or less left the expirery date will get 10 minutes added to its current time
        /// </summary>
        /// <param name="userId">The user id that is bidding</param>
        /// <param name="productId">The product id the user wants to bid on</param>
        /// <param name="amount">The amount the user want to pay</param>
        /// <returns>Returns ErrorCodes.BidTooLow if bid is too low, returns ErrorCodes.YourOwnProduct if user tries to bid on his own product
        /// else returns ErrorCodes.NoError</returns>
        public ErrorCodes BidOnProduct(int userId, int productId, decimal amount)
        {
            ProductModel<AuctionProductModel> product = auctionProduct.GetProduct(productId);
            if (product.Product.HighestBidder.Price >= amount)
            {
                return ErrorCodes.BidTooLow;
            }
            if (product.Owner.Id == userId)
            {
                return ErrorCodes.YourOwnProduct;
            }
            if (DateTime.Now.AddMinutes(10) >= product.Product.ExpireryDate)
            {
                product.Product.ExpireryDate = product.Product.ExpireryDate.AddMinutes(10);
            }
            auctionProduct.SetHighestBidder(userId, productId, amount);
            product.Product.HighestBidder.TriggerOnPriceChanged(true);
            return ErrorCodes.NoError;
        }

        /// <summary>
        /// Checks if the current time is greater or equals to the expire date of the product every 3 seconds
        /// if it is the status gets changed to sold and a event will be triggered
        /// </summary>
        private void CheckForFinnishedAuctions()
        {
            while (true)
            {
                List<ProductModel<AuctionProductModel>> products = auctionProduct.GetAll();
                for (int i = 0; i < products.Count; i++)
                {
                    DateTime productExpireDay = DateTime.Parse(products[i].Product.ExpireryDate.ToString());
                    if (DateTime.Now >= productExpireDay)
                    {
                        products[i].Product.Status = Interfaces.Status.SOLD;
                        products[i].Product.TriggerOnPriceChanged(products[i]);
                        auctionProduct.ChangeStatus(products[i].Product.Id, Interfaces.Status.SOLD);
                    }
                }

                Thread.Sleep(3000);
            }
        }
    }
}
