using H3AuctionHouse.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace H3AuctionHouse.Authorization
{
    /// <summary>
    /// This class is used for authorization pages
    /// </summary>
    public class JwtTokenValidateAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Checks to see if JWT token is expired or invalid using the ITokenService.ValidateToken method
        /// Redirects to logout if token is expired or invalid
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Gets token from header
            string token = context.HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(token))
            {
                //If token is null or empty, User is not logged in therefor we redirect to logout
                //Just to make sure cookies and session gets deleted
                context.Result = new RedirectResult("Logout");
                return;
            }
            token = token.Replace("Bearer ", "");
            ITokenService tokenManager = context.HttpContext.RequestServices.GetService(typeof(ITokenService)) as ITokenService;
            //Checks to see if token is invalid or expired using ITokenService
            if (tokenManager != null && !tokenManager.ValidateToken(token))
            {
                //Redirects to logout, if statement is true
                context.Result = new RedirectResult("Logout");
                return;
            }
        }
    }
}
