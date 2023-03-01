using AuctionHouseBackend;
using AuctionHouseBackend.Interfaces;
using AuctionHouseBackend.Managers;
using AuctionHouseBackend.Models;
using H3AuctionHouse.Authorization;
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

        [BindProperty]
        public decimal AutoBidValue { get; set; }
        [BindProperty]
        public decimal MaxAutobidValue { get; set; }
        public void OnGet(int id)
        {
            try
            {
                //Gets items with the id from our details page
                Item = Program.manager.Get<AuctionProductManager>().GetProduct(id);
                if(Item == null)
                {
                    Msg = "Could not get Item";
                    throw new Exception("Could not fetch item");
                }
            }
            catch (Exception e)
            {
                Logger.AddLog(AuctionHouseBackend.LogLevel.ERROR, "ItemDetails.OnGet()" + e.Message + e.StackTrace);
            }
        }
        public void OnPost(int id)
        {
            //The way we update Item is kinda stupid, idealy we want to bind the Item property, but it will be null for some reason
            //So the way we update Item needs to be refined
            //This is just to make it work
            AutobidModel autobid = null;
            UserModel user = HttpContext.Session.GetObjectFromJson<UserModel>("user");
            Item = Program.manager.Get<AuctionProductManager>().GetProduct(id);
            if (AutoBidValue > 0 && MaxAutobidValue > AutoBidValue)
            {
                autobid = new AutobidModel(user.Id, Item.Product.Id, AutoBidValue, MaxAutobidValue);
            }
            ResponseCode response = Program.manager.Get<AuctionProductManager>().BidOnProduct(user.Id, Item, BidValue, autobid);
            if(response == ResponseCode.NoError)
            {
                Program.manager.Get<AutobidManager>().Autobids.Add(autobid);
            }
            DisplayResponse(response);
            Item = Program.manager.Get<AuctionProductManager>().GetProduct(id);
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
