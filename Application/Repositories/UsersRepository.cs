using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Application.Models;
using server.Infrastructure.Context;

namespace server.Application.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ECommerceDBContext _context;
        private readonly SqlConnection connection;
        public UsersRepository(ECommerceDBContext context)
        {
            _context = context;
            connection = _context.Connection;
        }

        public async Task<IEnumerable<Customer>> GetCustomerListAsync()
        {
            IEnumerable<Customer> list = await _context.User
                .OfType<Customer>()
                .Where(u => u.Authentication!.ToLower() == "customer")
                .ToListAsync();
            return list;
        }
        public async Task<IEnumerable<User>> GetUserListsAsync()
        {
            IEnumerable<User> userLists = await _context.User.ToListAsync();
            return userLists;
        }
        public async Task<Customer?> GetCustomerAsync(int customerID)
        {
            Customer? searchedCustomer = await _context.User.OfType<Customer>().FirstOrDefaultAsync(eachCustomer => eachCustomer.ID == customerID);
            return searchedCustomer;
        }
        public async Task<User?> GetAdminAsync(int adminID)
        {
            User? searchedAdmin = await _context.User.FirstOrDefaultAsync(eachUser => eachUser.ID == adminID);
            return searchedAdmin;
        }
        public User? NewGetAdminAsync(int adminID)
        {
            User selectedUser = new User();
            SqlCommand command = new SqlCommand("GetAdminProc", connection);
            
            command.CommandType = System.Data.CommandType.StoredProcedure;
            connection.Open(); //Opening the connection so that the context will have an access to the SQL server.
            command.Parameters.Add(new SqlParameter("@AdminID",adminID));


            SqlDataReader reader = command.ExecuteReader(); //Excuting the stored procedure and reading the results.
            while (reader.Read())
            {
                selectedUser = new User(
                    reader["FirstName"].ToString()!,
                    reader["Lastname"].ToString()!,
                    reader["Email"].ToString()!,
                    reader["Password"].ToString()!,
                    reader["Authentication"].ToString()!);
            }

            //command.ExecuteNonQuery();
            connection.Close();
            return selectedUser;
        }
        public async Task CreateAdminAsync(User admin)
        {
            _context.User.Add(admin);
            await _context.SaveChangesAsync();
        }
        public async Task CreateCustomerAsync(Customer customer)
        {
            _context.User.Add(customer);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsEmailExisting(string email)
        {
            bool isExiting = await _context.User.AnyAsync(eachCustomer => eachCustomer.Email == email);
            return isExiting;
        }
        public async Task<User?> LoginUserAsync(string email)
        {
            User? searchedUser = await _context.User.FirstOrDefaultAsync(eachUser => eachUser.Email ==  email);
            return searchedUser;
        }
    }
}
