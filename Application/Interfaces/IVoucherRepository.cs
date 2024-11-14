using server.Application.Models;
using System.Reflection;

namespace server.Application.Interfaces
{
    public interface IVoucherRepository
    {
        Task<Voucher?> GetVoucherAsync(int customerID, int productID, int voucherID);
        Task CreateVoucherAsync(string Title, string Description, Product product, Customer customer, double Discount);
        Task ApplyVoucherAsync(AppliedVouchers appliedVoucher);
        Task<bool> VoucherCheckAsync(int cartID);
        Task<IEnumerable<Voucher?>> GetVoucherList(int cartProductID);
        Task<IEnumerable<Voucher?>> FilterVoucherList(int voucherID);
        Task RemoveVoucherAsync(int voucherID);
    }
}
