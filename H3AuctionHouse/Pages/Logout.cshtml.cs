using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H3AuctionHouse.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("user");
            Response.Cookies.Delete("token");
            return RedirectToPage("Index");
        }
    }
}
