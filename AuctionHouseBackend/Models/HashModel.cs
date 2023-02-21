
namespace AuctionHouseBackend.Models
{
    public class HashModel
    {
        public string Hash { get; private set; }
        public string Salt { get; private set; }

        public HashModel(string hash, string salt)
        {
            Hash = hash;
            Salt = salt;
        }
    }
}
