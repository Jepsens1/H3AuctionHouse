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
    public class AuctionProductManager : IProductManager<ProductModel<AuctionProductModel>>
    {
        private DatabaseAuctionProduct databaseAuctionProduct;
        public AuctionProductManager(DatabaseAuctionProduct databaseAuctionProduct) 
        { 
            this.databaseAuctionProduct = databaseAuctionProduct;
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

        List<ProductModel<AuctionProductModel>> IProductManager<ProductModel<AuctionProductModel>>.GetProduct(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
