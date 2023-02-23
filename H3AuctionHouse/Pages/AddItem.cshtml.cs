using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using AuctionHouseBackend.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using AuctionHouseBackend.Interfaces;

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

        public string Errormsg { get; set; }

        [BindProperty]
        public DateTime ExpireDate { get; set; } = DateTime.Now.AddMinutes(10);

        public SelectList Categorys { get; set; } = new SelectList(Enum.GetValues(typeof(Category)).Cast<Category>());

        [BindProperty]
        public string SelectedCategory { get; set; }


        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            try
            {
                bool Iscreated = false;
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
            catch (Exception)
            {
                Errormsg = "Could not create item";
                return Page();
            }
           
        }
    }
}
