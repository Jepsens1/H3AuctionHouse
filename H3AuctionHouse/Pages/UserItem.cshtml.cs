using AuctionHouseBackend;
using AuctionHouseBackend.Interfaces;
using AuctionHouseBackend.Managers;
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
        //Used to display all items that user has
        public List<ProductModel<AuctionProductModel>> UserItems { get; set; }
        
        //Used to display all categorys in dropdownmenu
        public SelectList Categorys { get; set; } = new SelectList(Enum.GetValues(typeof(Category)).Cast<Category>());

        [BindProperty(SupportsGet = true)]
        //Gets the value from dropdownmenu
        //SupportsGet needs to be true, otherwise the value is always null
        public string? SelectedCategory { get; set; }
        public string Errormsg { get; set; }
        public void OnGet()
        {
            try
            {
                //Gets user from session
                UserModel user = HttpContext.Session.GetObjectFromJson<UserModel>("user");
                if(user == null)
                {
                    throw new Exception("Session user is null");
                }
                //Gets the Users Items with user id
                UserItems = Program.manager.Get<AuctionProductManager>().GetUserProducts(user.Id);
                //If user selects category
                if (!string.IsNullOrEmpty(SelectedCategory))
                {
                    //Converts value from dropdownmenu to enum
                    Category category = (Category)Enum.Parse(typeof(Category), SelectedCategory);
                    //Gets users items with category selected
                    UserItems = Program.manager.Get<AuctionProductManager>().GetProduct(category);
                }
            }
            catch (Exception e)
            {
                Errormsg = "Something went wrong";
                Logger.AddLog(AuctionHouseBackend.LogLevel.ERROR, "UserItem.OnGet()" + e.Message + e.StackTrace);
            }
           
        }
    }
}
