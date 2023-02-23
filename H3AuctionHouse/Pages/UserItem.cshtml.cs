using AuctionHouseBackend.Interfaces;
using AuctionHouseBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace H3AuctionHouse.Pages
{
    [Authorize]
    public class UserItemModel : PageModel
    {
        public List<ProductModel<AuctionProductModel>> UserItems { get; set; }
        public void OnGet()
        {
            UserModel user = HttpContext.Session.GetObjectFromJson<UserModel>("user");
            UserItems = Program._auctionproductmanager.GetUserProducts(user.Id);
        }
    }
}
