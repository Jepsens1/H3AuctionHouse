using AuctionHouseBackend.Database;
using AuctionHouseBackend.Interfaces;
using AuctionHouseBackend.Managers;
using AuctionHouseBackend.Models;

namespace AuctionHouseBackend
{
    internal class Program
    {
        //static string con = "Server=PJJ-P15S-2022\\SQLEXPRESS;Database=AuctionHouse;Trusted_Connection=True;";
        static string con = "Server=DESKTOP-R394HDQ;Database=AuctionHouse;Trusted_Connection=True;";
        static List<ProductModel<AuctionProductModel>> auctionProducts;
        static void Main(string[] args)
        {
            AuctionProductManager product = new AuctionProductManager(new DatabaseAuctionProduct(con));
            CreateLoginUser("PJJ", "test1");
            LoginManager login = new LoginManager(new DatabaseLogin(con));
            UserModel user = login.GetUser("Jessen");
            auctionProducts = product.GetAll();
            for (int i = 0; i < auctionProducts.Count(); i++)
            {
                auctionProducts[i].Product.HighestBidder.OnPriceChanged += HighestBidder_OnPriceChanged;
            }
            while (true)
            {
                Console.WriteLine("1. Create product\n2. Get product\n3. Get all\n4. Bid on item");
                int input = Convert.ToInt32(Console.ReadLine());
                if (input == 1)
                {
                    ProductModel<AuctionProductModel> model = new ProductModel<AuctionProductModel>(new Models.AuctionProductModel("test jessen2", "test jessen2",
                        Category.KID, Status.AVAILABLE, DateTime.Today), user);
                    if (product.Create(model))
                    {
                        Console.WriteLine("Product created");
                    }
                }
                else if (input == 2)
                {
                    List<ProductModel<AuctionProductModel>> products = product.GetUserProducts(user.Id);
                    for (int i = 0; i < products.Count(); i++)
                    {
                        Console.WriteLine(products[i].Product.ToString());
                    }
                }
                else if (input == 3)
                {
                    List<ProductModel<AuctionProductModel>> products = product.GetAll();
                    for (int i = 0; i < products.Count(); i++)
                    {
                        Console.WriteLine(products[i].Product.ToString());
                        Console.WriteLine("Owner: " + products[i].Owner.Username);
                    }
                }
                else if (input == 4)
                {
                    product.BidOnProduct(user.Id, GetProductFromId(1), 700);
                }
            }
            
        }

        static private ProductModel<AuctionProductModel> GetProductFromId(int id)
        {
            for (int i = 0; i < auctionProducts.Count; i++)
            {
                if (auctionProducts[i].Product.Id == id)
                    return auctionProducts[i];
            }
            return null;
        }

        private static void HighestBidder_OnPriceChanged(object? sender, object e)
        {
            Console.WriteLine("price changed item id: " + ((ProductModel<AuctionProductModel>)e).Product.Id);
        }

        static void CreateLoginUser(string username, string mail)
        {
            LoginManager login = new LoginManager(new DatabaseLogin(con));
            Console.Read();
            if (!login.CreateAccount(new Models.UserModel("Patrick", "Jessen", username, mail, "test1234")))
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