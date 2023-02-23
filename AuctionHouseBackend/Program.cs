using AuctionHouseBackend.Database;
using AuctionHouseBackend.Interfaces;
using AuctionHouseBackend.Managers;
using AuctionHouseBackend.Models;

namespace AuctionHouseBackend
{
    internal class Program
    {
        static string con = "Server=PJJ-P15S-2022\\SQLEXPRESS;Database=AuctionHouse;Trusted_Connection=True;";
        static void Main(string[] args)
        {
            AuctionProductManager product = new AuctionProductManager(new DatabaseAuctionProduct(con));
            CreateLoginUser("Jessen", "test2");
            LoginManager login = new LoginManager(new DatabaseLogin(con));
            UserModel user = login.GetUser("Jessen");
            while (true)
            {
                Console.WriteLine("1. Create product\n2. Get product\n3. Get all");
                int input = Convert.ToInt32(Console.ReadLine());
                if (input == 1)
                {
                    ProductModel<AuctionProductModel> model = new ProductModel<AuctionProductModel>(new Models.AuctionProductModel("test jessen2", "test jessen2",
                        Interfaces.Category.KID, Interfaces.Status.CREATED, DateTime.Today), user);
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
            }
            
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