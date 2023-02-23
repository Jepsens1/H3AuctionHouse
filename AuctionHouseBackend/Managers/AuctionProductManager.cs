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
    /// <summary>
    /// This object handles the logic for all the auctions
    /// Once instatiated a thread will be started which checks for finnished auctions an event will be fired from [AuctionProductModel.cs] if auction has finnished
    /// </summary>
    public class AuctionProductManager : IProductManager<ProductModel<AuctionProductModel>>
    {
        List<ProductModel<AuctionProductModel>> Products { get; set; }
        private DatabaseAuctionProduct databaseAuctionProduct;
        public AuctionProductManager(DatabaseAuctionProduct databaseAuctionProduct) 
        { 
            this.databaseAuctionProduct = databaseAuctionProduct;
            Thread t = new Thread(CheckForFinnishedAuctions);
            t.Start();
        }

        public List<ProductModel<AuctionProductModel>> GetAll()
        {
            return databaseAuctionProduct.GetAll();
        }

        public bool Create(ProductModel<AuctionProductModel> product)
        {
            return databaseAuctionProduct.Create(product);
        }

        public List<ProductModel<AuctionProductModel>> GetUserProducts(int userId)
        {
            return databaseAuctionProduct.GetUserProducts(userId);
        }

        /// <summary>
        /// Function for the user to bid on products
        /// If user bids on product with 10 minutes or less left the expirery date will get 10 minutes added to its current time
        /// Event from [AuctionBidderModel.cs] will be triggered when price is changed
        /// </summary>
        /// <param name="userId">The user id that is bidding</param>
        /// <param name="productId">The product id the user wants to bid on</param>
        /// <param name="amount">The amount the user want to pay</param>
        /// <returns>Returns ErrorCodes.BidTooLow if bid is too low, returns ErrorCodes.YourOwnProduct if user tries to bid on his own product
        /// else returns ErrorCodes.NoError</returns>
        public ErrorCodes BidOnProduct(int userId, ProductModel<AuctionProductModel> product, decimal amount)
        {
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
                product.Product.ExpireryDate = DateTime.Now.AddMinutes(10);
            }
            databaseAuctionProduct.SetHighestBidder(userId, product.Product.Id, amount);
            product.Product.HighestBidder.TriggerOnPriceChanged(product);
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
                Products = databaseAuctionProduct.GetAll();
                for (int i = 0; i < Products.Count; i++)
                {
                    DateTime productExpireDay = DateTime.Parse(Products[i].Product.ExpireryDate.ToString());
                    if (DateTime.Now >= productExpireDay)
                    {
                        Products[i].Product.Status = Interfaces.Status.SOLD;
                        Products[i].Product.TriggerOnStatusChanged(Products[i]);
                        databaseAuctionProduct.ChangeStatus(Products[i].Product.Id, Interfaces.Status.SOLD);
                    }
                }

                Thread.Sleep(3000);
            }
        }
    }
}
