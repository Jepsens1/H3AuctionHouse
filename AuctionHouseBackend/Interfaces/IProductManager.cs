using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Interfaces
{
    public interface IProductManager<T>
    {
        List<T> GetAll();
        bool Create(T product);
        List<T> GetUserProducts(int userId);
        List<T> GetProduct(Category category);
    }
}
