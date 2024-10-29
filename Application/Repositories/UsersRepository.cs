using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Application.Models;
using server.Infrastructure.Context;

namespace server.Application.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ECommerceDBContext _context;
        public UsersRepository(ECommerceDBContext context)
        {
            _context = context;
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
