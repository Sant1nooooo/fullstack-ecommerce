namespace server.Application.Models
{
    public class Customer : User
    {
        public double Balance { get; set; }
        public int Points { get; set; }
        public bool IsMember { get; set; }

        public Customer(string FirstName, string LastName, string Email, string Password) : base(FirstName, LastName, Email, Password, "Customer")
        {
            Balance = 0;
            Points = 0;
            IsMember = false;
        }
    }
}
