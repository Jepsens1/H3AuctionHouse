using AuctionHouseBackend;
using AuctionHouseBackend.Models;
using Ganss.Xss;
namespace H3AuctionHouse
{
    /// <summary>
    /// This Class is used to sanitize the UserModel from database, before we output to website
    /// </summary>
    public class InputSanitizer : IInputSanitizer
    {

        /// <summary>
        /// Sanitize all UserModel property, and removes all danger tags 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Sanitized User Model</returns>
        public UserModel SanitizeInputLogin(UserModel model)
        {
            try
            {
                HtmlSanitizer sanitizer = new HtmlSanitizer();
                model.Username = sanitizer.Sanitize(model.Username);
                model.Password = sanitizer.Sanitize(model.Password);
                model.FirstName = sanitizer.Sanitize(model.FirstName);
                model.LastName = sanitizer.Sanitize(model.LastName);
                model.Email = sanitizer.Sanitize(model.Email);

                return model;
            }
            catch (Exception e)
            {
                Logger.AddLog(AuctionHouseBackend.LogLevel.ERROR, "InputSanitizer.SanitizeInputLogin()" + e.Message + e.StackTrace);
                throw;
            }
        }
    }
}
