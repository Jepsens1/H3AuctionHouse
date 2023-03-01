using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using AuctionHouseBackend.Models;
using AuctionHouseBackend;
using AuctionHouseBackend.Managers;

namespace H3AuctionHouse.Pages
{
    public class RegisterModel : PageModel
    {
        [Required]
        [BindProperty, StringLength(20, MinimumLength = 5, ErrorMessage = "Username no longer than 20 and less than 5")]
        public string Username { get; set; }

        [Required]
        [BindProperty, StringLength(20, MinimumLength = 8, ErrorMessage = "Password no longer than 16 and less than 8")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //Used to compare Password property and ConfirmPassword property
        [BindProperty, Required, Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
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
                if(!ModelState.IsValid)
                {
                    return Page();
                }
                //If true returns to Login page
                if (Program.manager.Get<AccountManager>().CreateAccount(new UserModel(Firstname, Lastname, Username, Email, Password)))
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
