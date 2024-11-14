using MediatR;
using server.Application.Interfaces;
using server.Application.Models;
using server.Application.Repositories;
using static server.Core.ResponseModels;

namespace server.Application.Command_Operations.CartProduct
{
    public class ApplyVoucher_Command : IRequest<ApplyVoucher_Result>
    {
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public int VoucherID { get; set; }
        public bool ToogleVoucher { get; set; }
    }

    public class ApplyVoucher_CommandHandler : IRequestHandler<ApplyVoucher_Command, ApplyVoucher_Result>
    {
        private readonly IUsersRepository _users;
        private readonly IProductRepository _product;
        private readonly ICustomerRepository _customer;
        private readonly IVoucherRepository _voucher;
        public ApplyVoucher_CommandHandler(IUsersRepository users, IProductRepository product, ICustomerRepository customer, IVoucherRepository voucher)
        {
            _users = users;
            _product = product;
            _customer = customer;
            _voucher = voucher;
        }
        public async Task<ApplyVoucher_Result> Handle(ApplyVoucher_Command request, CancellationToken ct)
        {
            Product? product = await _product.GetProductAsync(request.ProductID);
            Customer? customer = await _users.GetCustomerAsync(request.CustomerID);
            if (product is null || customer is null)
            {
                return new ApplyVoucher_Result
                {
                    IsApplied = true,
                    Message = product is null ? "WARNING: ProductID does not exist!" : "WARNING: CustomerID does not exist!"
                };
            }

            CartProducts? selectedCartProduct = await _customer.GetCartProductsAsync(request.ProductID, request.CustomerID);
            if(selectedCartProduct is null ) return new ApplyVoucher_Result { IsApplied = true, Message = $"WARNING: You don't have {product.ProductName} in your cart!" };

            Voucher? selectedVoucher = await _voucher.GetVoucherAsync(request.CustomerID, request.ProductID, request.VoucherID);
            if (selectedVoucher is null) return new ApplyVoucher_Result { IsApplied = true, Message = $"WARNING: You don't have voucher for {product.ProductName}!" };
            Console.WriteLine($"SELECTED VOUCHER: {selectedVoucher.Title}");




            string responseMessage = "";
            double discount = selectedVoucher.Discount;
            if (request.ToogleVoucher)
            {
                if (selectedVoucher.IsUsed) return new ApplyVoucher_Result { IsApplied = true, Message = "WARNING: You already used this voucher!" };
                
                bool IsFirst = await _voucher.VoucherCheckAsync(selectedCartProduct.ID);
                if (IsFirst) //Baka icheck nalang yung length ng IEnumerable<Voucher?> and i-remove yung `VoucherCheckAsync()`
                {

                    IEnumerable<Voucher?> voucherList = await _voucher.GetVoucherList(selectedCartProduct.ID);
                    discount += voucherList.Sum(eachVoucher => eachVoucher!.Discount);//New voucher is already incldued.
                }

                selectedVoucher.IsUsed = true;
                await _voucher.ApplyVoucherAsync(new AppliedVouchers() { CartProduct = selectedCartProduct, Voucher = selectedVoucher});
                responseMessage = $"You've applied voucher to {product.ProductName}";
            }
            else
            {
                IEnumerable<Voucher?> voucherList = await _voucher.FilterVoucherList(request.VoucherID); //Not included the target voucher.
                discount = voucherList.Sum(eachVoucher => eachVoucher!.Discount);

                selectedVoucher.IsUsed = false;
                await _voucher.RemoveVoucherAsync(request.VoucherID);
                responseMessage = "Voucher removed successfully!";
            }
             
            discount /= 100;
            selectedCartProduct.DiscountedPrice = (int)(selectedCartProduct.OriginalPrice - (discount * selectedCartProduct.OriginalPrice));

            Console.WriteLine($"CALCULATED DISCOUNT PERCENTAGE: {discount}");

            await _product.UpdateChanges();
            return new ApplyVoucher_Result() { IsApplied = true, Message = responseMessage};
        }
    }
}
