using server.Application.Models;
using System.Reflection;

namespace server.Application.Interfaces
{
    public interface IVoucherRepository
    {
        Task<Voucher?> GetVoucherAsync(int customerID, int productID);
        Task CreateVoucherAsync(string Title, string Description, Product product, Customer customer, double Discount);
    }
}
