﻿using server.Application.Models;

namespace server.Application.Interfaces
{
    public interface IUsersRepository
    {
        Task<IEnumerable<Customer>> GetCustomerListAsync();
        Task<IEnumerable<User>> GetUserListsAsync();
        Task<Customer?> GetCustomerAsync(int customerID);
        Task<User?> GetAdminAsync(int adminID);
        Task CreateAdminAsync(User admin);
        Task CreateCustomerAsync(Customer customer);
        Task<bool> IsEmailExisting(string email);
        Task<User?> LoginUserAsync(string email);
        Task<User?> NewGetAdminAsync(int adminID);
    }
}
