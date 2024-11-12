using server.Application.Models;
using MediatR;
using server.Application.Interfaces;
using static server.Core.ResponseModels;

namespace server.Application.Query_Operations.Vouchers
{
    public class GetVoucher_Command : IRequest<GetVoucher_Result>
    {
        public int CustomerID { get; set; }
        public int ProductID {  get; set; }
    }
    public class GetVoucher_CommandHandler : IRequestHandler<GetVoucher_Command, GetVoucher_Result>
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IProductRepository _productRepository;
        public GetVoucher_CommandHandler(IVoucherRepository voucherRepository, IUsersRepository usersRepository, IProductRepository productRepository)
        {
            _voucherRepository = voucherRepository;
            _usersRepository = usersRepository;
            _productRepository = productRepository;
        }
        public async Task<GetVoucher_Result> Handle(GetVoucher_Command request, CancellationToken ct)
        {
            Customer? selectedCustomer = await _usersRepository.GetCustomerAsync(request.CustomerID);
            Product? selectedProduct= await _productRepository.GetProductAsync(request.ProductID);

            if (selectedCustomer is null ) return new GetVoucher_Result() { IsRetrieved = false, ErrorMessage = "WARNING: Invalid CustomerID!" };
            if (selectedProduct is null) return new GetVoucher_Result() { IsRetrieved = false, ErrorMessage = "WARNING: Invalid ProductID!" };


            Voucher? selectedVoucher =  await _voucherRepository.GetVoucherAsync(request.CustomerID,request.ProductID);
            if (selectedVoucher is null) return new GetVoucher_Result() { IsRetrieved = false, ErrorMessage = "WARNING: You don't have voucher for this product!"};
            if (selectedVoucher.IsUsed) return new GetVoucher_Result() { IsRetrieved = false, ErrorMessage = "WARNING: You already used this voucher!" };

            return new GetVoucher_Result() { IsRetrieved = true, Voucher = selectedVoucher};
        }
    }
}
