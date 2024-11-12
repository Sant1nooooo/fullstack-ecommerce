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
        public bool ToogleVoucher { get; set; }
    }

    public class ApplyVoucher_CommandHandler : IRequestHandler<ApplyVoucher_Command, ApplyVoucher_Result>
    {
        private readonly IUsersRepository _users;
        private readonly IProductRepository _product;
        private readonly ICustomerRepository _customer;
        private readonly IVoucherRepository _voucher;
        //Voucher repository
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
            string responseMessage = "";
            if (product is null || customer is null)
            {
                return new ApplyVoucher_Result
                {
                    IsApplied = true,
                    Message = product is null ? "WARNING: ProductID does not exist!" : "WARNING: CustomerID does not exist!"
                };
            }
            //Check muna kung may specific product yung Customer doon sa `CartProducts` table before retreiving the voucher for that product.
            CartProducts? selectedCartProduct = await _customer.GetCartProductsAsync(request.ProductID, request.CustomerID);
            if(selectedCartProduct is null ) return new ApplyVoucher_Result { IsApplied = true, Message = $"WARNING: You don't have {product.ProductName} in your cart!" };

            Voucher? selectedVoucher = await _voucher.GetVoucherAsync(request.CustomerID, request.ProductID);
            if (selectedVoucher is null) return new ApplyVoucher_Result { IsApplied = true, Message = $"WARNING: You don't have voucher for {product.ProductName}!" };

            if (request.ToogleVoucher)
            {
                if(selectedVoucher.IsUsed) return new ApplyVoucher_Result { IsApplied = true, Message = "WARNING: You already used this voucher!" };
                double discount = selectedVoucher.Discount / 100;
                selectedCartProduct.DiscountedPrice = (int)(selectedCartProduct.OriginalPrice - (discount * selectedCartProduct.OriginalPrice));
                selectedCartProduct.Voucher = selectedVoucher;
                selectedVoucher.IsUsed = true;
                responseMessage = $"You've applied voucher to {product.ProductName}";
            }
            else
            {
                selectedCartProduct.DiscountedPrice = 0;
                selectedCartProduct.Voucher = null;
                selectedVoucher.IsUsed = false;
                responseMessage = "Voucher removed successfully!";
            }

            await _product.UpdateChanges();
            return new ApplyVoucher_Result() { IsApplied = true, Message = responseMessage};
        }
    }
}
