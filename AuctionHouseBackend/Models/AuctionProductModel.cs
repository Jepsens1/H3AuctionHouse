using AuctionHouseBackend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Models
{
    public class AuctionProductModel : IProduct
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public Status Status { get; set; }
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
    }
}
