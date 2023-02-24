using AuctionHouseBackend;
using AuctionHouseBackend.Interfaces;
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

        public List<ProductModel<AuctionProductModel>> Items { get; set; }

        public SelectList Categorys { get; set; } = new SelectList(Enum.GetValues(typeof(Category)).Cast<Category>());

        [BindProperty(SupportsGet = true)]
        public string? SelectedCategory { get; set; }

        public string Errormsg { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
            try
            {
                Items = Program._auctionproductmanager.GetAll();
                if (!string.IsNullOrEmpty(SelectedCategory))
                {
                    Category category = (Category)Enum.Parse(typeof(Category), SelectedCategory);
                    Items = Program._auctionproductmanager.GetProduct(category);
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