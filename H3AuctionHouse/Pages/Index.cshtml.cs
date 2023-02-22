using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace H3AuctionHouse.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<int> Items { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public async Task OnGetAsync()
        {   
            
            Items = new List<int>();
            Items.Add(1);
            Items.Add(2);
            Items.Add(3);
            Items.Add(4);
            Items.Add(5);
            Items.Add(6);
            Items.Add(7);
            
            //Items = Program._manager.GetItems()
        }
    }
}