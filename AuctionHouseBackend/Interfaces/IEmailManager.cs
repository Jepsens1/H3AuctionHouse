using AuctionHouseBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Interfaces
{
    public interface IEmailManager
    {
        void SendMail(UserModel to, string subject, string body);
    }
}
