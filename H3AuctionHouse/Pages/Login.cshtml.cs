using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuctionHouseBackend.Models;
using AuctionHouseBackend;

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
                UserModel user = Program._loginManager.Login(Username, Password);
                if (user != null)
                {
                    HttpContext.Session.SetObjectAsJson("user", user);
                    string token = BuildToken();
                    CookieOptions options = new CookieOptions
                    {
                        Secure = true,
                        HttpOnly = true,
                    };
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

        private string BuildToken()
        {
            List<Claim> userclaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Username),
            };
            //useroles needs to be added
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisismySecretKey"));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
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
