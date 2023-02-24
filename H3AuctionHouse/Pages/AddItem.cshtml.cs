using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using AuctionHouseBackend.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using AuctionHouseBackend.Interfaces;
using AuctionHouseBackend;

namespace H3AuctionHouse.Pages
{
    [Authorize]
    public class AddItemModel : PageModel
    {
        [Required]
        [BindProperty]
        public string ProductName { get; set; }

        [Required]
        [BindProperty]
        public string Description { get; set; }

        //Used for error message on screen
        public string Errormsg { get; set; }

        //Used for setting when item should expire
        [BindProperty]
        public DateTime ExpireDate { get; set; } = DateTime.Now.AddMinutes(10);

        //This is the dropdown menu we see on screen
        public SelectList Categorys { get; set; } = new SelectList(Enum.GetValues(typeof(Category)).Cast<Category>());

        [BindProperty]
        //By default the value is first value of our dropdown menu
        public string SelectedCategory { get; set; }


        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            try
            {
                bool Iscreated = false;
                //Gets our user by our session
                UserModel user = HttpContext.Session.GetObjectFromJson<UserModel>("user");

                Category category = (Category)Enum.Parse(typeof(Category), SelectedCategory);
                if(!string.IsNullOrEmpty(ProductName) && !string.IsNullOrEmpty(Description))
                {
                     Iscreated = Program._auctionproductmanager.Create(new ProductModel<AuctionProductModel>(new AuctionProductModel(ProductName, Description, category
                    , Status.AVAILABLE, ExpireDate), user));

                }

                if (!Iscreated)
                {
                    Errormsg = "Could not create item";
                    return Page();
                }
                return RedirectToPage("Index");

            }
            catch (Exception e)
            {
                Errormsg = "Could not create item";
                Logger.AddLog(AuctionHouseBackend.LogLevel.ERROR, "AddItem.OnPost()" + e.Message + e.StackTrace);
                return Page();
            }
           
        }
    }
}
