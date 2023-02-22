using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Interfaces
{
    // What category the product belongs in
    public enum Category { CAR, FOOD, JEWLERY, KID, DIVERSE };
    // Status of the product
    public enum Status { SOLD, CANCELLED, IN_PROGRESS, CREATED };
    /// <summary>
    /// Product class represents a product a user wanna put up for sell, buy, leasing or put it up on auction
    /// </summary>
    public interface IProduct
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public Category Category { get; }
        public Status Status { get; }
    }
}
