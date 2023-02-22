namespace AuctionHouseBackend.Models
{
    /// <summary>
    /// This class represents a user
    /// </summary>
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // Username is unique wich is handled by the database
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public HashModel Hash { get; set; }

        // Used to registre a user
        public UserModel(string firstName, string lastName, string username, string email, string password) 
        { 
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Email = email;
            Password = password;
        }

        // Used to login a user
        public UserModel(string username, HashModel hash) 
        { 
            Username = username;
            Hash = hash;
        }

        public UserModel(int id)
        {
            Id = id;
        }
    }
}
