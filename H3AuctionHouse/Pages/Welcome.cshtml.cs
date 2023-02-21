using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H3AuctionHouse.Pages
{
    [Authorize]
    public class WelcomeModel : PageModel
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public void OnGet()
        {
            Username = HttpContext.Session.GetString("username");
            Token = Request.Cookies["token"];
        }
        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Remove("username");
            Response.Cookies.Delete("token");
            return RedirectToPage("Index");
        }
    }
}
