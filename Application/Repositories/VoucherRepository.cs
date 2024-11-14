using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Application.Models;
using server.Infrastructure.Context;

namespace server.Application.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly ECommerceDBContext _context;
        private readonly SqlConnection connection;
        public VoucherRepository(ECommerceDBContext context)
        {
            _context = context;
            connection = _context.Connection; //Initlaizing the SQLConnection
        }
        public async Task<Voucher?> GetVoucherAsync(int customerID, int productID, int voucherID)
        {
            Voucher? voucher = await _context.Vouchers
                .Where(v => v.Customer!.ID == customerID && v.Product!.ID == productID && v.ID == voucherID)
                .Include(v => v.Product)
                .Include(v => v.Customer)
                .FirstOrDefaultAsync();
                //.FirstOrDefaultAsync(x => x.ID == voucherID);
            return voucher;
        }
        public async Task CreateVoucherAsync(string title, string description, Product product, Customer customer, double discount)
        {

            SqlParameter titleParam = new SqlParameter("@title", title);
            SqlParameter descriptionParam = new SqlParameter("@description", description);
            SqlParameter productIDParam = new SqlParameter("@ProductID", product.ID);
            SqlParameter customerIDParam = new SqlParameter("@CustomerID", customer.ID);
            SqlParameter discountParam = new SqlParameter("@discount", discount);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC CreateVoucher @title, @description, @ProductID, @CustomerID, @discount",
                titleParam, descriptionParam, productIDParam, customerIDParam, discountParam);

            ////ExecuteRawSqlAsync
            //SqlCommand command = new SqlCommand("CreateVoucher", connection);
            //command.CommandType = System.Data.CommandType.StoredProcedure;

            //command.Parameters.Add(new SqlParameter("@title",title));
            //command.Parameters.Add(new SqlParameter("@description", description));
            //command.Parameters.Add(new SqlParameter("@ProductID", product.ID));
            //command.Parameters.Add(new SqlParameter("@CustomerID", customer.ID));
            //command.Parameters.Add(new SqlParameter("@discount", discount));

            //await connection.OpenAsync();
            //await command.ExecuteNonQueryAsync();
            //await connection.CloseAsync();
        }
        public async Task ApplyVoucherAsync(AppliedVouchers appliedVoucher)
        {
            _context.AppliedVouchers.Add(appliedVoucher);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> VoucherCheckAsync(int cartID)
        {
            bool isFirst = await _context.AppliedVouchers
                .AnyAsync(av => av.CartProduct != null && av.CartProduct.ID == cartID);
            return isFirst;
        }
        public async Task<IEnumerable<Voucher?>> GetVoucherList(int cartProductID)
        {
            IEnumerable<Voucher?> voucherList = await _context.AppliedVouchers
                .Where(av => av.CartProduct!.ID == cartProductID)
                .Select(av => av.Voucher)
                .ToListAsync();

            return voucherList;
        }
        public async Task<IEnumerable<Voucher?>> FilterVoucherList(int voucherID)
        {
            IEnumerable<Voucher?> filteredVoucherList = await _context.AppliedVouchers
                .Where(av => av.Voucher!.ID != voucherID)
                .Select(av => av.Voucher)
                .ToListAsync();
            return filteredVoucherList;
        }
        public async Task RemoveVoucherAsync(int voucherID)
        {
            AppliedVouchers? rowData = await _context.AppliedVouchers
                .Where(av => av.Voucher!.ID == voucherID)
                .FirstOrDefaultAsync();

            _context.AppliedVouchers.Remove(rowData!);

            await _context.SaveChangesAsync();
        }
    }
}
