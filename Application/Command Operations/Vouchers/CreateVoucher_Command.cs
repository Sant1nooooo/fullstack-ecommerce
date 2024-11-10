using MediatR;
using server.Application.Interfaces;
using server.Application.Models;
using static server.Core.ResponseModels;

namespace server.Application.Command_Operations.Vouchers
{
    public class CreateVoucher_Command : IRequest<CreateVoucher_Result>
    {
        public int ProductID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public double Discount { get; set; }
        public bool IsMember{ get; set; }


        /*
            "Special discount for members only! Enjoy a limited-time discount on selected products!"
            "Everyone saves! Enjoy discounts on select products, available to all customers."
         */
    }

    public class CreateVoucher_CommandHandler : IRequestHandler<CreateVoucher_Command, CreateVoucher_Result>
    {
        private readonly IUsersRepository _user;
        private readonly IProductRepository _product;
        private readonly IVoucherRepository _voucher;
        public CreateVoucher_CommandHandler(IUsersRepository user, IProductRepository product, IVoucherRepository voucher)
        {
            _user = user;
            _product = product;
            _voucher = voucher;
        }
        public async Task<CreateVoucher_Result> Handle(CreateVoucher_Command request, CancellationToken ct)
        {
            IEnumerable<Customer> customerList = await _user.GetCustomerListAsync();
            Product? selectedProduct = await _product.GetProductAsync(request.ProductID);

            if (customerList is null) return new CreateVoucher_Result() { IsCreated = false, Message = "WARNING: CustomerList is empty!"};
            if(selectedProduct is null) return new CreateVoucher_Result() { IsCreated = false, Message = "WARNING: ProductID is invalid!" };
            
            //Filtering if the voucher will be made for either Customer(member) or Customer(Not Member).
            IEnumerable<Customer> targetCustomer = request.IsMember 
                ? customerList.Where(customer => customer.IsMember)
                : customerList;

            foreach (Customer eachCustomer in targetCustomer)
            {
                await _voucher.CreateVoucherAsync(request.Title!, request.Description!, selectedProduct, eachCustomer, request.Discount);
            }

            return new CreateVoucher_Result() { IsCreated = true, Message = $"Voucher for {selectedProduct.ProductName}"};
        }
    }
}
