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
    public class ProductManager : IProductManager<ProductModel<SaleProductModel>>
    {

        private DatabaseProduct databaseProduct;
        public ProductManager(DatabaseProduct databaseProduct)
        {
            this.databaseProduct = databaseProduct;
        }
        public bool Create(ProductModel<SaleProductModel> product)
        {
            throw new NotImplementedException();
        }

        public List<ProductModel<SaleProductModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public ProductModel<AuctionProductModel> GetProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public List<ProductModel<SaleProductModel>> GetUserProducts(int userId)
        {
            throw new NotImplementedException();
        }

        public void AddImages(byte[] img, int productId)
        {
            databaseProduct.InsertImg(img, productId);
        }
    }
}
