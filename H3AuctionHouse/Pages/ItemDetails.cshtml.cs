using AuctionHouseBackend;
using AuctionHouseBackend.Interfaces;
using AuctionHouseBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace H3AuctionHouse.Pages
{
    public class ItemDetailsModel : PageModel
    {
        //Used to display our items
        public ProductModel<AuctionProductModel> Item { get; set; }

        //Used to get value from input box
        [BindProperty]
        public decimal BidValue { get; set; }

        //Used to display message on bid status
        public string Msg { get; set; }
        public void OnGet(int id)
        {
            try
            {
                //Gets items with the id from our details page
                Item = Program._auctionproductmanager.GetProduct(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void OnPost(int id)
        {
            //The way we update Item is kinda stupid, idealy we want to bind the Item property, but it will be null for some reason
            //So the way we update Item needs to be refined
            //This is just to make it work
            Item = Program._auctionproductmanager.GetProduct(id);
            UserModel user = HttpContext.Session.GetObjectFromJson<UserModel>("user");
            ResponseCode response = Program._auctionproductmanager.BidOnProduct(user.Id, Item, BidValue);
            DisplayResponse(response);
            Item = Program._auctionproductmanager.GetProduct(id);
        }
        /// <summary>
        /// Used to display message if bid is accepted or not
        /// </summary>
        /// <param name="response"></param>
        private void DisplayResponse(ResponseCode response)
        {
            switch (response)
            {
                case ResponseCode.NoError:
                    Msg = "Bid is made";
                    break;
                case ResponseCode.BidTooLow:
                    Msg = "Your bid is lower then what is currently bid";
                    break;
                case ResponseCode.YourOwnProduct:
                    Msg = "You cant bid on your own product";
                    break;
                case ResponseCode.ProductSold:
                    Msg = "Product is already sold";
                    break;
            }
        }
    }
}
