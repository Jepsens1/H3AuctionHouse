using AuctionHouseBackend.Cryption;
using AuctionHouseBackend.Database;
using AuctionHouseBackend.Models;

namespace AuctionHouseBackend.Managers
{
    public class AccountManager
    {
        private DatabaseHelper databaseHandler;

        public AccountManager(DatabaseHelper databaseHandler)
        {
            this.databaseHandler = databaseHandler;
        }

        public UserModel GetUser(string username)
        {
            return databaseHandler.GetUser(username).Result;
        }

        public UserModel Login(string username, string password)
        {
            UserModel user = ((DatabaseLogin)databaseHandler).Login(username).Result;
            if (user == null)
            {
                return null;
            }
            if (!CryptoService.VerifyPassword(password, user.Hash.Hash, user.Hash.Salt))
            {
                return null;
            }
            HashModel newHashSalt = CryptoService.SaltPassword(password);
            ((DatabaseLogin)databaseHandler).UpdateLogin(user.Id, newHashSalt);
            return user;
        }

        public bool CreateAccount(UserModel user)
        {
            HashModel hashSalt = CryptoService.SaltPassword(user.Password);
            user.Hash = hashSalt;
            if (((DatabaseLogin)databaseHandler).CreateAccount(user).Result)
            {
                return true;
            }
            return false;
        }
    }
}
