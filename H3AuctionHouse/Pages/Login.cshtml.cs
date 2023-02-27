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

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            try
            {
                //Tries to login
                UserModel user = Program.manager.Get<AccountManager>().Login(Username, Password);
                if (user != null)
                {
                    //Sets session with our user object
                    HttpContext.Session.SetObjectAsJson("user", user);
                    //Builds a JWT token
                    string token = BuildToken();
                    CookieOptions options = new CookieOptions
                    {
                        Secure = true,
                        HttpOnly = true,
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
        /// <summary>
        /// This method returns a JWT token as string
        /// </summary>
        /// <returns></returns>
        private string BuildToken()
        {
            //JWT token will contain the username
            List<Claim> userclaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Username),
            };
            //Needs to be reworked, as the key should be in appsettings rather than hardcoded
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisismySecretKey"));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            JwtSecurityToken token = new JwtSecurityToken(
                //Issuer and audience should be found in appsettings
                issuer: "test",
                audience: "test",
                claims: userclaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
