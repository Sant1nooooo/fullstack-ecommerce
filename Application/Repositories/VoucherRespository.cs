using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Application.Models;
using server.Infrastructure.Context;

namespace server.Application.Repositories
{
    public class VoucherRespository : IVoucherRepository
    {
        private readonly ECommerceDBContext _context;
        private readonly SqlConnection connection;
        public VoucherRespository(ECommerceDBContext context)
        {
            _context = context;
            connection = _context.Connection;
        }
        public async Task<Voucher?> GetVoucherAsync(int customerID, int productID)
        {
            Voucher? voucher = await _context.Vouchers
                .Where(v => v.Customer!.ID == customerID && v.Product!.ID == productID)
                .Include(v => v.Product)
                .Include(v => v.Customer)
                .FirstOrDefaultAsync();
                //.FirstOrDefaultAsync(x => x.ID == voucherID);
            return voucher;
        }
        
        public async Task CreateVoucherAsync(string title, string description, Product product, Customer customer, double discount)
        {
            SqlCommand command = new SqlCommand("CreateVoucher", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@title",title));
            command.Parameters.Add(new SqlParameter("@description", description));
            command.Parameters.Add(new SqlParameter("@ProductID", product.ID));
            command.Parameters.Add(new SqlParameter("@CustomerID", customer.ID));
            command.Parameters.Add(new SqlParameter("@discount", discount));

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }
    }
}
