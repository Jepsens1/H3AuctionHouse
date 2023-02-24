using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using AuctionHouseBackend.Models;
using AuctionHouseBackend;

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
        [Required]
        [BindProperty]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [BindProperty]
        public string Firstname { get; set; }
        [Required]
        [BindProperty]
        public string Lastname { get; set; }
        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            try
            {
                if (Program._loginManager.CreateAccount(new UserModel(Firstname, Lastname, Username, Email, Password)))
                {
                    return RedirectToPage("Login");
                }
                else
                {
                    return Page();
                }
            }
            catch (Exception e)
            {
                Logger.AddLog(AuctionHouseBackend.LogLevel.ERROR, "Register.OnPost()" + e.Message + e.StackTrace);
                return Page();
            }
            
        }
    }
}
