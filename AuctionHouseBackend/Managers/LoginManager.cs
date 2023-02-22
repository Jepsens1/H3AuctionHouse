using AuctionHouseBackend.Cryption;
using AuctionHouseBackend.Database;
using AuctionHouseBackend.Models;

namespace AuctionHouseBackend.Managers
{
    public class LoginManager
    {
        private DatabaseHandler databaseHandler;

        public LoginManager(DatabaseHandler databaseHandler)
        {
            this.databaseHandler = databaseHandler;
        }

        public UserModel GetUser(string username)
        {
            return databaseHandler.GetUser(username);
        }

        public bool Login(string username, string password)
        {
            UserModel user = ((DatabaseLogin)databaseHandler).Login(username);
            if (user == null)
            {
                return false;
            }
            if (!CryptoService.VerifyPassword(password, user.Hash.Hash, user.Hash.Salt))
            {
                return false;
            }
            HashModel newHashSalt = CryptoService.SaltPassword(password);
            ((DatabaseLogin)databaseHandler).UpdateLogin(user.Id, newHashSalt);
            return true;
        }

        public bool CreateAccount(UserModel user)
        {
            HashModel hashSalt = CryptoService.SaltPassword(user.Password);
            user.Hash = hashSalt;
            if (((DatabaseLogin)databaseHandler).CreateAccount(user))
            {
                return true;
            }
            return false;
        }
    }
}
