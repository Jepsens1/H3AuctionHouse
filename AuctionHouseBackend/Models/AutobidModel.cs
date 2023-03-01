using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Models
{
    public class AutobidModel
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public decimal AutobidPrice { get; set; }
        public decimal AutobidMax { get; set;}

        public AutobidModel(int userId, int productId, decimal autobidPrice, decimal autobidMax)
        {
            UserId = userId;
            ProductId = productId;
            AutobidPrice = autobidPrice;
            AutobidMax = autobidMax;
        }
    }
}
