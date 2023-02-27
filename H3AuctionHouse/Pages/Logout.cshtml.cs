using AuctionHouseBackend;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H3AuctionHouse.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            try
            {
                //Removes session and deletes cookie
                HttpContext.Session.Remove("user");
                Response.Cookies.Delete("token");
                return RedirectToPage("Index");
            }
            catch (Exception e)
            {
                Logger.AddLog(AuctionHouseBackend.LogLevel.ERROR, "Logout.OnGet()" + e.Message + e.StackTrace);
                return RedirectToPage("Index");
            }
        }
    }
}
