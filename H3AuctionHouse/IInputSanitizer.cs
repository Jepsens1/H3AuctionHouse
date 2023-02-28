using AuctionHouseBackend.Models;

namespace H3AuctionHouse
{
    public interface IInputSanitizer
    {
        UserModel SanitizeInputLogin(UserModel model);
    }
}