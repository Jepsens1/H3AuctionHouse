using AuctionHouseBackend;
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
        public SelectList Categorys { get; set; } = new SelectList(Enum.GetValues(typeof(Category)).Cast<Category>());

        [BindProperty(SupportsGet = true)]
        public string? SelectedCategory { get; set; }
        public string Errormsg { get; set; }
        public void OnGet()
        {
            try
            {
                UserModel user = HttpContext.Session.GetObjectFromJson<UserModel>("user");
                UserItems = Program._auctionproductmanager.GetUserProducts(user.Id);
                if (!string.IsNullOrEmpty(SelectedCategory))
                {
                    Category category = (Category)Enum.Parse(typeof(Category), SelectedCategory);
                    UserItems = Program._auctionproductmanager.GetProduct(category);
                }
            }
            catch (Exception e)
            {
                Errormsg = "Something went wrong";
                Logger.AddLog(AuctionHouseBackend.LogLevel.ERROR, "UserItem.OnGet()" + e.Message + e.StackTrace);
                throw;
            }
           
        }
    }
}
