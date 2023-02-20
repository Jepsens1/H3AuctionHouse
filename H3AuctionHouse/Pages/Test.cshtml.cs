using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H3AuctionHouse.Pages
{
    public class TestModel : PageModel
    {
        public string MyProperty { get; set; }
        public void OnGet()
        {
            MyProperty = "Hello World";
        }
    }
}
