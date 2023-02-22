using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Models
{
    public class AuctionBidderModel
    {
        public UserModel User { get; set; }
        public decimal Price { get; set; }

        public AuctionBidderModel(UserModel user, decimal price)
        {
            User = user;
            Price = price;
        }
    }
}
