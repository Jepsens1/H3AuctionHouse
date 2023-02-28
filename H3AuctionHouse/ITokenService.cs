namespace H3AuctionHouse
{
    public interface ITokenService
    {
        string BuildToken(string username);
        bool ValidateToken(string token);
    }
}