using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Interfaces
{
    /// <summary>
    /// Product manager interface used for different types of product managers
    /// </summary>
    /// <typeparam name="T">The type of product</typeparam>
    public interface IProductManager<T>
    {
        // Used to get all products
        List<T> GetAll();
        // Used to Create a new product
        bool Create(T product);
        // Used to get a product posted by a specific user
        List<T> GetUserProducts(int userId);
    }
}
