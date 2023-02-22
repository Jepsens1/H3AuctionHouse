using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H3AuctionHouse.Pages
{
    public class ItemDetailsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Item { get; set; }
        public void OnGet(int id)
        {
            Item = id;
        }
    }
}
