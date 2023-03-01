using H3AuctionHouse.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace H3AuctionHouse.Authorization
{
    public class JwtTokenValidateAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers["Authorization"];
            token = token.Replace("Bearer ", "");
            if (string.IsNullOrWhiteSpace(token))
            {
                context.Result = new RedirectResult("Logout");
                return;
            }
            ITokenService tokenManager = context.HttpContext.RequestServices.GetService(typeof(ITokenService)) as ITokenService;
            if (tokenManager != null && !tokenManager.ValidateToken(token))
            {
                context.Result = new RedirectResult("Logout");
                return;
            }
        }
    }
}
