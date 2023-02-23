using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend
{
    public enum ErrorCodes
    {
        NoError,
        UserExists,
        BidTooLow,
        NoUserFound
    }
}
