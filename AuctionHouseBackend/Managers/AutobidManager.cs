using System;
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
        public List<AutobidModel> Autobids { get; set; }
        private DatabaseAuctionProduct databaseAuctionProduct;
        private List<ProductModel<AuctionProductModel>> products;

        public AutobidManager(DatabaseAuctionProduct databaseAuctionProduct, AuctionProductManager productManager)
        {
            this.databaseAuctionProduct = databaseAuctionProduct;
            this.products = productManager.Products;
            Autobids = databaseAuctionProduct.GetAutobids().Result;
            Thread t = new Thread(() => Autobid(productManager));
            t.Start();
        }

        /// <summary>
        /// Checks if a user has already made an autobid
        /// Used so we can update the Autobid instead of making a new
        /// </summary>
        /// <param name="product"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool HasUserAutobid(ProductModel<AuctionProductModel> product, UserModel user)
        {
            for (int i = 0; i < Autobids.Count; i++)
            {
                if (Autobids[i].UserId == user.Id && Autobids[i].ProductId == product.Product.Id)
                {
                    return true;
                }
            }
            return false;
        }

        public void UpdateAutobid(AutobidModel autobid)
        {
            databaseAuctionProduct.UpdateAutoBid(autobid.UserId, autobid.ProductId, autobid.AutobidPrice, autobid.AutobidMax);
        }

        public void Autobid(AuctionProductManager productManager)
        {
            while (true)
            {
                for (int i = 0; i < products.Count; i++)
                {
                    for (int j = 0; j < Autobids.Count; j++)
                    {
                        if (Autobids[j].ProductId == products[i].Product.Id && Autobids[j].UserId != products[i].Product.HighestBidder.User.Id)
                        {
                            decimal price = GetAutobidPrice(i, j);
                            if (price > 0 && price > products[i].Product.HighestBidder.Price)
                            {
                                productManager.BidOnProduct(Autobids[j].UserId, products[i], price);
                            }
                        }
                    }
                }
                Thread.Sleep(2000);
            }
        }

        private decimal GetAutobidPrice(int i, int j)
        {
            if (Autobids[j].AutobidMax > products[i].Product.HighestBidder.Price)
            {
                return Autobids[j].AutobidPrice + products[i].Product.HighestBidder.Price;
            }
            return -1;
        }
    }
}