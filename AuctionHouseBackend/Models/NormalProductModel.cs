using AuctionHouseBackend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Models
{
    /// <summary>
    /// This object represents a normal project that isnt up for auction but just for sale
    /// </summary>
    public class SaleProductModel : PropertyChangedModel, IProduct
    {
        public event EventHandler<object> OnStatusChanged;
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        private Status status;
        public Status Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
        public DateTime ExpireryDate { get; set; }
        public AuctionBidderModel HighestBidder { get; set; }

        public List<string> Imgs { get; set; }



        public void TriggerOnStatusChanged(object product)
        {
            OnStatusChanged?.Invoke(this, product);
        }
    }
}
