using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace H3AuctionHouse
{
    public class TokenService : ITokenService
    {
        /// <summary>
        /// This method returns a JWT token as string
        /// </summary>
        /// <returns></returns>
        public string BuildToken(string username)
        {
            //JWT token will contain the username
            List<Claim> userclaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
            };
            //Needs to be reworked, as the key should be in appsettings rather than hardcoded
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisismySecretKey"));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            JwtSecurityToken token = new JwtSecurityToken(
                //Issuer and audience should be found in appsettings
                issuer: "test",
                audience: "test",
                claims: userclaims,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        /// <summary>
        /// Validates a JWT token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool ValidateToken(string token)
        {
            //Needs to be reworked, as the key should be in appsettings rather than hardcoded
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisismySecretKey"));
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            try
            {
                handler.ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = "test",
                        ValidAudience = "test",
                        IssuerSigningKey = key,
                        // set clockskew to zero so tokens expire exactly at token expiration time 
                        //By default ClockSkew is set to 5 minutes
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedtoken);
                var jwtToken = (JwtSecurityToken)validatedtoken;
                //If null we know that token has expired or is invalid
                if(jwtToken == null)
                {
                    return false;
                }
                return true;
            }
            //Needs to work on error handling
            catch (Exception)
            {
                return false;
            }
        }
    }
}
