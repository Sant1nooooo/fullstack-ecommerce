using MediatR;
using server.Application.Interfaces;
using server.Application.Models;
using static server.Core.ResponseModels;

namespace server.Application.Command_Operations.Products
{
    public class UpdateProductVisibility_Command : IRequest<UpdateProductVisibility_Result>
    {
        public int productID { get; set; }
    }

    public class UpdateProductVisibility_CommandHandler : IRequestHandler<UpdateProductVisibility_Command, UpdateProductVisibility_Result>
    {
        private readonly IProductRepository _productRepository;
        public UpdateProductVisibility_CommandHandler(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public async Task<UpdateProductVisibility_Result> Handle(UpdateProductVisibility_Command request, CancellationToken ct)
        {
            Product? selectedProduct = await _productRepository.GetProductAsync(request.productID);

            if (selectedProduct is null) return new UpdateProductVisibility_Result() { IsSuccessful = false, Message = "WARNNG: ProductID does not exist!"};

            selectedProduct.IsAvailable = !selectedProduct.IsAvailable; //Toggling true or  false.

            await _productRepository.UpdateChanges();

            return new UpdateProductVisibility_Result() { IsSuccessful = false, Message = $"({selectedProduct.ProductName}) hidden successfully!" };
        }
    }
}
