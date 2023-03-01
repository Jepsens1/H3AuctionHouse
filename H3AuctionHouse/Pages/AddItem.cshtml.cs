using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using AuctionHouseBackend.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using AuctionHouseBackend.Interfaces;
using AuctionHouseBackend;
using AuctionHouseBackend.Managers;

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
        public string ErrorMessage { get; set; }

        //Used for setting when item should expire, default value is now + 10 minutes
        [BindProperty]
        public DateTime ExpireDate { get; set; } = DateTime.Now.AddMinutes(10);

        //This is the dropdown menu we see on screen
        public SelectList Categorys { get; set; } = new SelectList(Enum.GetValues(typeof(Category)).Cast<Category>());

        [BindProperty]
        //Used for which category the item is
        //By default the value is first value of our dropdown menu
        public string SelectedCategory { get; set; }


        public void OnGet()
        {
        }
        /// <summary>
        /// Method is called when user clicks submit button
        /// Method creates new Item in database
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            try
            {
                //Checks to see if user has filled all input fields
                if(!ModelState.IsValid)
                {
                    return Page();
                }
                bool Iscreated = false;
                //Gets user by session
                UserModel user = HttpContext.Session.GetObjectFromJson<UserModel>("user");
                if(user== null)
                {
                    throw new Exception("Session user is null");
                }
                //Convets the SelectedCategory string to Enum
                Category category = (Category)Enum.Parse(typeof(Category), SelectedCategory);
                //Checks to see if ProductName and Description is not empty or null
                if(!string.IsNullOrEmpty(ProductName) && !string.IsNullOrEmpty(Description))
                {
                    //Gets true or false on if item is added to database
                     Iscreated = Program.manager.Get<AuctionProductManager>().Create(new ProductModel<AuctionProductModel>(new AuctionProductModel(ProductName, Description, category
                    , Status.AVAILABLE, ExpireDate), user));

                }
                if (!Iscreated)
                {
                    ErrorMessage = "Could not create item";
                    return Page();
                }
                return RedirectToPage("Index");

            }
            catch (Exception e)
            {
                ErrorMessage = "Could not create item";
                Logger.AddLog(AuctionHouseBackend.LogLevel.ERROR, "AddItem.OnPost()" + e.Message + e.StackTrace);
                return Page();
            }
           
        }
    }
}
