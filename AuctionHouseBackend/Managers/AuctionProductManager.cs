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
    public class AuctionProductManager : IProductManager<AuctionProductModel>
    {
        private DatabaseAuctionProduct databaseAuctionProduct;
        public AuctionProductManager(DatabaseAuctionProduct databaseAuctionProduct) 
        { 
            this.databaseAuctionProduct = databaseAuctionProduct;
        }

        public List<AuctionProductModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Create(AuctionProductModel product)
        {
            return databaseAuctionProduct.Create(product);
        }

        public List<AuctionProductModel> GetUserProducts(int userId)
        {
            return databaseAuctionProduct.GetUserProducts(userId);
        }

        List<AuctionProductModel> IProductManager<AuctionProductModel>.GetProduct(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
