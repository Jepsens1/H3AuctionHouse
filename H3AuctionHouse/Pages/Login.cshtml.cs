using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if(Username == "admin" && Password == "admin")
            {
                HttpContext.Session.SetString("username", Username);
                string token = BuildToken();
                Response.Cookies.Append("token", token);
                return RedirectToPage("Welcome");
            }
            else
            {
                return Page();
            }
        }

        private string BuildToken()
        {
            List<Claim> userclaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Username),
                new Claim(ClaimTypes.Role, "Admin")
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
