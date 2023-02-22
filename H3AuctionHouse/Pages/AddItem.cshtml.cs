using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

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

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            return Page();
        }
    }
}
