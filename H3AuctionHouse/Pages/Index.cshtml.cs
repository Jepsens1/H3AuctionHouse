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
        [Required]
        public string SelectedCategory { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
            Items = Program._auctionproductmanager.GetAll();
            if(!string.IsNullOrEmpty(SelectedCategory))
            {
                //Items = Program._auctionproductmanager.
                //select from db where category
            }
        }
    }
}