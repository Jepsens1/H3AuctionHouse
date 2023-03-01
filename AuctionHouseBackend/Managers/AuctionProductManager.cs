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
        public event EventHandler<object> OnProductCreated;
        public List<ProductModel<AuctionProductModel>> Products { get; set; }
        private DatabaseAuctionProduct databaseAuctionProduct;
        public AuctionProductManager(DatabaseAuctionProduct databaseAuctionProduct) 
        { 
            this.databaseAuctionProduct = databaseAuctionProduct;
            Products = databaseAuctionProduct.GetAll().Result;
            Thread t = new Thread(CheckForFinnishedAuctions);
            t.Start();
        }

        public List<ProductModel<AuctionProductModel>> GetAll()
        {
            return databaseAuctionProduct.GetAll().Result;
        }

        public bool Create(ProductModel<AuctionProductModel> product)
        {
            bool create = databaseAuctionProduct.Create(product).Result;
            if (create)
            {
                Products.Add(product);
                TriggerOnProductCreated(product);
            }
            return create;
        }

        public List<ProductModel<AuctionProductModel>> GetUserProducts(int userId)
        {
            return databaseAuctionProduct.GetUserProducts(userId).Result;
        }

        public ProductModel<AuctionProductModel> GetProduct(int productId)
        {
            return databaseAuctionProduct.GetProduct(productId).Result;
        }

        public List<ProductModel<AuctionProductModel>> GetProduct(Category category)
        {
            List<ProductModel<AuctionProductModel>> categoryProducts = new List<ProductModel<AuctionProductModel>>();
            for (int i = 0; i < Products.Count; i++)
            {
                if (Products[i].Product.Category == category)
                {
                    categoryProducts.Add(Products[i]);
                }
            }
            return categoryProducts;
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
        public ResponseCode BidOnProduct(int userId, ProductModel<AuctionProductModel> product, decimal amount, AutobidModel autobid = null)
        {
            try
            {
                ResponseCode response = HandleBidResponseCodes(userId, product, amount);
                if (response != ResponseCode.NoError)
                {
                    return response;
                }
                LastMinuteBid(product);
                databaseAuctionProduct.SetHighestBidder(userId, product.Product.Id, amount);
                if (autobid != null)
                {
                    databaseAuctionProduct.AddAutobid(userId, product.Product.Id, autobid.AutobidPrice, autobid.AutobidMax);
                }
                product.Product.HighestBidder.Price = amount;
                product.Product.HighestBidder.User = databaseAuctionProduct.GetUser(userId).Result;
                product.Product.HighestBidder.TriggerOnPriceChanged(product);
            }
            catch (Exception ex)
            {
                Logger.AddLog(LogLevel.ERROR, "BidOnProduct()" + ex.Message);
                return ResponseCode.UnknownError;
            }
            return ResponseCode.NoError;
        }

        private void LastMinuteBid(ProductModel<AuctionProductModel> product)
        {
            if (DateTime.Now.AddMinutes(-1) > product.Product.ExpireryDate)
            {
                product.Product.ExpireryDate = product.Product.ExpireryDate.AddMinutes(1);
                databaseAuctionProduct.UpdateExpireryDate(product.Product.Id, product.Product.ExpireryDate);
            }
        }

        private ResponseCode HandleBidResponseCodes(int userId, ProductModel<AuctionProductModel> product, decimal amount)
        {
            if (product.Owner.Id == userId)
            {
                return ResponseCode.YourOwnProduct;
            }
            if (product.Product.Status == Status.SOLD)
            {
                return ResponseCode.ProductSold;
            }
            if (product.Product.Status == Status.EXPIRED)
            {
                return ResponseCode.ProductExpired;
            }
            if (product.Product.HighestBidder.Price >= amount)
            {
                return ResponseCode.BidTooLow;
            }
            return ResponseCode.NoError;
        }

        /// <summary>
        /// Checks if the current time is greater or equals to the expire date of the product every 2 seconds
        /// if it is the status gets changed to sold and a event will be triggered
        /// </summary>
        private void CheckForFinnishedAuctions()
        {
            while (true)
            {
                for (int i = 0; i < Products.Count; i++)
                {
                    DateTime productExpireDay = DateTime.Parse(Products[i].Product.ExpireryDate.ToString());
                    if (DateTime.Now >= productExpireDay && Products[i].Product.Status != Status.SOLD && Products[i].Product.Status != Status.EXPIRED)
                    {
                        if (Products[i].Product.HighestBidder != null && Products[i].Product.HighestBidder.Price > 0)
                        {
                            Products[i].Product.Status = Status.SOLD;
                            databaseAuctionProduct.ChangeStatus(Products[i].Product.Id, Status.SOLD);
                        }
                        else
                        {
                            Products[i].Product.Status = Status.EXPIRED;
                            databaseAuctionProduct.ChangeStatus(Products[i].Product.Id, Status.EXPIRED);
                        }
                        Products[i].Product.TriggerOnStatusChanged(Products[i]);
                    }
                }

                Thread.Sleep(2000);
            }
        }

        private void TriggerOnProductCreated(object product)
        {
            OnProductCreated?.Invoke(this, product);
        }
    }
}
