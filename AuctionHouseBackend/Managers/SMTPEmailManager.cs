using AuctionHouseBackend.Interfaces;
using AuctionHouseBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Managers
{
    public class SMTPEmailManager : IEmailManager, IManager
    {

        public void SendMail(UserModel to, string subject, string body)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("I got the mail", "i got the password"),
                EnableSsl = true,
            };

            smtpClient.Send("i got the mail", to.Email, subject, body);
        }
    }
}
