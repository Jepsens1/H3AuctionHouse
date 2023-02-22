using AuctionHouseBackend.Database;
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
            //CreateLoginUser();
            LoginManager login = new LoginManager(new DatabaseLogin(con));
            UserModel user = login.GetUser("Jessen2");
            while (true)
            {
                Console.WriteLine("1. Create product\n2. Get product");
                int input = Convert.ToInt32(Console.ReadLine());
                if (input == 1)
                {
                    if (product.Create(new Models.AuctionProductModel("test jessen2", "test jessen2", Interfaces.Category.KID, Interfaces.Status.CREATED,
                        DateTime.Today, DateTime.Today, user)))
                    {
                        Console.WriteLine("Product created");
                    }
                }
                else if (input == 2)
                {
                    List<AuctionProductModel> products = product.GetUserProducts(user.Id);
                    for (int i = 0; i < products.Count(); i++)
                    {
                        Console.WriteLine(products[i].ToString());
                    }
                }
            }
            
        }

        static void CreateLoginUser()
        {
            LoginManager login = new LoginManager(new DatabaseLogin(con));
            while (true)
            {
                Console.WriteLine("1. Create account\n2. Login");
                int input = Convert.ToInt32(Console.ReadLine());

                if (input == 1)
                {
                    Console.WriteLine("add user");
                    Console.Read();
                    if (!login.CreateAccount(new Models.UserModel("Patrick", "Jessen", "PJJ", "pjj@mail.dk", "test1234")))
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
                else if (input == 2)
                {
                    Console.WriteLine("Login");
                    Console.WriteLine("Username: ");
                    string username = Console.ReadLine();
                    Console.WriteLine("Password: ");
                    string password = Console.ReadLine();
                    if (login.Login(username, password))
                    {
                        Console.WriteLine("Sucessfully logged in!");
                        Console.Read();
                    }
                    else
                    {
                        Console.WriteLine("Failed to login");
                        Console.Read();
                    }
                }

            }
        }
    }
}