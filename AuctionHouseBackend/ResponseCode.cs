using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend
{
    public enum ResponseCode
    {
        NoError,
        UnknownError,
        BidTooLow,
        ProductSold,
        ProductExpired,
        YourOwnProduct
    }
}
