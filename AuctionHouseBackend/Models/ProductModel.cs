using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Models
{
    /// <summary>
    /// Scaleable ProductModel object that can be any type of product
    /// </summary>
    /// <typeparam name="IProduct">Product type</typeparam>
    public class ProductModel<IProduct>
    {
        public IProduct Product { get; set; }
        public UserModel Owner { get; set; }

        public ProductModel(IProduct product, UserModel owner)
        {
            Product = product;
            Owner = owner;
        }
    }
}
