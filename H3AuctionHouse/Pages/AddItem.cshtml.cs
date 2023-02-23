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
        [Required]
        public DateTime ExpireDate { get; set; } = DateTime.Now;

        public SelectList Categorys { get; set; } = new SelectList(Enum.GetValues(typeof(Category)).Cast<Category>());

        [BindProperty]
        [Required]
        public string SelectedCategory { get; set; }


        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            try
            {
                UserModel user = HttpContext.Session.GetObjectFromJson<UserModel>("user");
                Category category = (Category)Enum.Parse(typeof(Category), SelectedCategory);
                bool Iscreated = Program._auctionproductmanager.Create(new ProductModel<AuctionProductModel>(new AuctionProductModel(ProductName, Description, category
                    , Status.CREATED, ExpireDate), user));

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
