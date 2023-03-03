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
    public class SMTPEmailManager : IEmailManager
    {

        public void SendMail(UserModel to, string subject, string body)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                UseDefaultCredentials = false,
                // email should be replaced with the email sender and with its password
                Credentials = new NetworkCredential("email", "password"),
                EnableSsl = true,
            };
            // email should be replaced with the email sender
            smtpClient.Send("email", to.Email, subject, body);
        }
    }
}
