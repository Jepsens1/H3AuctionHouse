using AuctionHouseBackend.Database;
using AuctionHouseBackend.Interfaces;
using AuctionHouseBackend.Managers;
using AuctionHouseBackend.Models;

namespace AuctionHouseBackend
{
    /// <summary>
    /// FOR TEST PURPOSE ONLY
    /// </summary>
    internal class Program
    {
        static string con = "Server=PJJ-P15S-2022\\SQLEXPRESS;Database=AuctionHouse;Trusted_Connection=True;";
        //static string con = "Server=DESKTOP-R394HDQ;Database=AuctionHouse;Trusted_Connection=True;";
        static Manager manager = new Manager(con);
        static AccountManager login = new AccountManager(new DatabaseLogin(con));
        static UserModel user = login.GetUser("Jessen2");
        static void Main(string[] args)
        {
            CreateLoginUser(manager, "Jessen3", "test4");
            //UserModel d = login.GetUser("pjj");
            for (int i = 0; i < manager.Get<AuctionProductManager>().Products.Count(); i++)
            {
                manager.Get<AuctionProductManager>().Products[i].Product.HighestBidder.OnPriceChanged += HighestBidder_OnPriceChanged;
            }
            while (true)
            {
                Console.WriteLine("1. Create product\n2. Get product\n3. Get all\n4. Bid on item");
                int input = Convert.ToInt32(Console.ReadLine());
                if (input == 1)
                {
                    ProductModel<AuctionProductModel> model = new ProductModel<AuctionProductModel>(new Models.AuctionProductModel("test jessen2", "test jessen2",
                        Category.KID, Status.AVAILABLE, DateTime.Now.AddMinutes(2000)), user);
                    if (manager.Get<AuctionProductManager>().Create(model))
                    {
                        Console.WriteLine("Product created");
                    }
                }
                else if (input == 2)
                {
                    List<ProductModel<AuctionProductModel>> products = manager.Get<AuctionProductManager>().GetUserProducts(user.Id);
                    for (int i = 0; i < products.Count(); i++)
                    {
                        Console.WriteLine(products[i].Product.ToString());
                    }
                }
                else if (input == 3)
                {
                    List<ProductModel<AuctionProductModel>> products = manager.Get<AuctionProductManager>().GetAll();
                    for (int i = 0; i < products.Count(); i++)
                    {
                        Console.WriteLine(products[i].Product.ToString());
                        Console.WriteLine("Owner: " + products[i].Owner.Username);
                    }
                }
                else if (input == 4)
                {
                    ProductModel<AuctionProductModel> product = GetProductFromId(1);
                    AutobidModel autobid = new AutobidModel(user.Id, product.Product.Id, 20, 7000);
                    manager.Get<AuctionProductManager>().BidOnProduct(user.Id, product, 300, autobid);
                    manager.Get<AutobidManager>().Autobids.Add(autobid);
                }
                else if (input == 5)
                {
                    ProductModel<AuctionProductModel> product = GetProductFromId(1);
                    manager.Get<AuctionProductManager>().BidOnProduct(user.Id, product, Convert.ToDecimal(Console.ReadLine()));
                }
            }
            
        }

        static private ProductModel<AuctionProductModel> GetProductFromId(int id)
        {
            for (int i = 0; i < manager.Get<AuctionProductManager>().Products.Count; i++)
            {
                if (manager.Get<AuctionProductManager>().Products[i].Product.Id == id)
                    return manager.Get<AuctionProductManager>().Products[i];
            }
            return null;
        }

        private static void HighestBidder_OnPriceChanged(object? sender, object e)
        {
            Console.WriteLine("price changed to " + ((ProductModel<AuctionProductModel>)e).Product.HighestBidder.Price);
        }

        static void CreateLoginUser(Manager manager, string username, string mail)
        {
            Console.Read();
            if (!manager.Get<AccountManager>().CreateAccount(new Models.UserModel("Patrick", "Jessen", username, mail, "1")))
            {
                Console.WriteLine("Username already exists");
                Console.Read();
            }
            else
            {
                Console.WriteLine("Account created");
                Console.Read();
            }
        }
    }
}