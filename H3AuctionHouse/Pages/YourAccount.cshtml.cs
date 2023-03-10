using AuctionHouseBackend.Models;
using H3AuctionHouse.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H3AuctionHouse.Pages
{
    //Uses JWTToken validation to see if token is expired or invalid
    [JwtTokenValidate]
    public class YourAccountModel : PageModel
    {
        public UserModel MyUser { get; set; }
        public string ErrorMsg { get; set; }
        public void OnGet()
        {
            try
            {
                MyUser = HttpContext.Session.GetObjectFromJson<UserModel>("user");
                if(MyUser == null)
                {
                    throw new Exception("Could not fetch token");
                }
            }
            catch (Exception)
            {
                ErrorMsg = "Your not logged in";
            }
        }
    }
}
