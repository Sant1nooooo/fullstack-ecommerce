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
    }
}
