using AuctionHouseBackend.Database;
using AuctionHouseBackend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseBackend.Managers
{
    /// <summary>
    /// Manager class that collects all the managers for easy access
    /// This class can handle other objects than managers but its main purpose is the managers only
    /// </summary>
    public class Manager
    {
        List<object> managers;

        public Manager(string constring)
        {
            managers= new List<object>();
            DatabaseAuctionProduct databaseAuctionProduct = new DatabaseAuctionProduct(constring);
            Add<AuctionProductManager>(new AuctionProductManager(databaseAuctionProduct));
            Add<AccountManager>(new AccountManager(new DatabaseLogin(constring)));
            Add<SMTPEmailManager>(new SMTPEmailManager());
            Add<AutobidManager>(new AutobidManager(databaseAuctionProduct, Get<AuctionProductManager>()));
        }

        /// <summary>
        /// Get a manager object of type specified
        /// </summary>
        /// <typeparam name="T">Type of manager object</typeparam>
        /// <returns>the manager object</returns>
        public T? Get<T>()
        {
            for (int i = 0; i < managers.Count; i++)
            {
                if (managers[i].GetType() == typeof(T))
                {
                    return (T)managers[i];
                }
            }
            return default(T);
        }

        private void Add<T>(T manager)
        {
            managers.Add(manager);
        }
    }
}
