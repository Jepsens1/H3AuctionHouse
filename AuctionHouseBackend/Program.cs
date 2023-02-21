using AuctionHouseBackend.Database;
using AuctionHouseBackend.Managers;

namespace AuctionHouseBackend
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LoginManager login = new LoginManager(new DatabaseLogin("Server=PJJ-P15S-2022\\SQLEXPRESS;Database=AuctionHouse;Trusted_Connection=True;"));

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