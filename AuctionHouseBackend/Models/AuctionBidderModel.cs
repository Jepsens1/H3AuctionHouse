using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Models
{
    public class AuctionBidderModel : PropertyChangedModel
    {
        public event EventHandler<object> OnPriceChanged;
        public UserModel User { get; set; }
        private decimal price;
        public decimal Price
        {
            get { return price; }
            set 
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public AuctionBidderModel(UserModel user, decimal price)
        {
            User = user;
            Price = price;
        }

        public void TriggerOnPriceChanged(object product)
        {
            OnPriceChanged?.Invoke(this, product);
        }
    }
}
