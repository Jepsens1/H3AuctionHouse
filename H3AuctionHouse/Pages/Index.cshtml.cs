using AuctionHouseBackend;
using AuctionHouseBackend.Interfaces;
using AuctionHouseBackend.Managers;
using AuctionHouseBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace H3AuctionHouse.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        //Used to display items on screen
        public List<ProductModel<AuctionProductModel>> Items { get; set; }

        //This is the dropdown menu we see on screen
        public SelectList Categorys { get; set; } = new SelectList(Enum.GetValues(typeof(Category)).Cast<Category>());

        //Used for filter the items
        //SupportsGet needs to be true, otherwise the value is always null
        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; }

        //Used to display error message on site
        public string Errormsg { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
            try
            {
                //Gets all items
                Items = Program.manager.Get<AuctionProductManager>().GetAll();
                //If user selects category
                if (!string.IsNullOrEmpty(SelectedCategory))
                {
                    //Converts value from dropdown to enum
                    Category category = (Category)Enum.Parse(typeof(Category), SelectedCategory);
                    //Finds all items with category selected
                    Items = Program.manager.Get<AuctionProductManager>().GetProduct(category);
                }
            }
            catch (Exception e)
            {
                Errormsg = "something went wrong";
                Logger.AddLog(AuctionHouseBackend.LogLevel.ERROR, "Index.OnGet()" + e.Message + e.StackTrace);
            }
        }
    }
}