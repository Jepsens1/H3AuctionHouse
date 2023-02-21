using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace H3AuctionHouse.Pages
{
    public class RegisterModel : PageModel
    {
        [Required]
        [BindProperty]
        public string Username { get; set; }

        [Required]
        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            if (Username == "admin" && Password == "admin")
            {
                return RedirectToPage("Login");
            }
            else
            {
                return Page();
            }
        }
    }
}
