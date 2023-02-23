using AuctionHouseBackend.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Models
{
    public class AuctionProductModel : PropertyChangedModel, IProduct
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

        public AuctionProductModel(int id, string name, string description, Category category, Status status, DateTime expireryDate)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            Status = status;
            ExpireryDate = expireryDate;
        }

        public AuctionProductModel(string name, string description, Category category, Status status, DateTime expireryDate)
        {
            Name = name;
            Description = description;
            Category = category;
            Status = status;
            ExpireryDate = expireryDate;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Description: {Description}, Category: {Category.ToString()}, Status: {Status.ToString()}, " +
                $"Expirery Date: {ExpireryDate}, Price: {HighestBidder.Price}";
        }

        public void TriggerOnStatusChanged(object product)
        {
            OnStatusChanged?.Invoke(this, product);
        }
    }
}
