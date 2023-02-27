﻿using AuctionHouseBackend.Database;
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
    /// </summary>
    public class Manager
    {
        List<object> managers;

        public Manager(string constring)
        {
            managers= new List<object>();
            Add<AuctionProductManager>(new AuctionProductManager(new DatabaseAuctionProduct(constring)));
            Add<AccountManager>(new AccountManager(new DatabaseLogin(constring)));
            Add<SMTPEmailManager>(new SMTPEmailManager());
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
                    return ((T)managers[i]);
                }
            }
            return default(T);
        }

        public void Add<T>(T manager)
        {
            managers.Add(manager);
        }
    }
}
