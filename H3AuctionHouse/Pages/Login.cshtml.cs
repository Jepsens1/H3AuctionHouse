using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuctionHouseBackend.Models;
using AuctionHouseBackend;
using AuctionHouseBackend.Managers;

namespace H3AuctionHouse.Pages
{
    public class LoginModel : PageModel
    {
       
        [Required]
        [BindProperty]
        public string Username { get; set; }

        [Required]
        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Errormsg { get; set; }

        private IInputSanitizer _saniz;
        private ITokenService _service;

        public LoginModel(IInputSanitizer sanitizer, ITokenService service)
        {
            _saniz = sanitizer;
            _service = service;
        }

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
                //Tries to login
                UserModel user = Program.manager.Get<AccountManager>().Login(Username, Password);
                if (user != null)
                {
                    user = _saniz.SanitizeInputLogin(user);
                    //Sets session with our user object
                    HttpContext.Session.SetObjectAsJson("user", user);
                    //Builds a JWT token
                    string token = _service.BuildToken(user.Username);
                    CookieOptions options = new CookieOptions
                    {
                        Secure = true,
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                    };
                    //Creates a cookie with our jwt used for authorization
                    Response.Cookies.Append("token", token, options);
                    return RedirectToPage("Index");
                }
                else
                {
                    Errormsg = "Wrong username or password";
                    return Page();
                }
            }
            catch (Exception e)
            {
                Errormsg = "Something went wrong";
                Logger.AddLog(AuctionHouseBackend.LogLevel.ERROR, "Login.OnPost()" + e.Message + e.StackTrace);
                return Page();
            }
          
        }
    }
}
