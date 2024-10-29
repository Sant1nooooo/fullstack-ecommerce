using MediatR;
using server.Application.Interfaces;
using server.Application.Models;
using static server.Core.ResponseModels;

namespace server.Application.Command_Operations.Products
{
    public class DiscountProduct_Command : IRequest<DiscountedProduct_Result>
    {
        public int ProductID { get; set; }
        public int Discount { get; set; }
    }
    public class DiscountProduct_CommandHandler : IRequestHandler<DiscountProduct_Command, DiscountedProduct_Result>
    {
        private readonly IProductRepository _productRepository;
        public DiscountProduct_CommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<DiscountedProduct_Result> Handle(DiscountProduct_Command request, CancellationToken ct)
        {
            Product? selectedProduct = await _productRepository.GetProductAsync(request.ProductID);


            if (selectedProduct is null) return new DiscountedProduct_Result() { IsSuccessful = false, Message = "WARNING: ProductID does not exist!" };

            decimal discount = request.Discount / 100m; //0.5
            selectedProduct.Discount = request.Discount; //50
            selectedProduct.DiscountedPrice = (int)(selectedProduct.OriginalPrice - (selectedProduct.OriginalPrice * discount)); //500
            await _productRepository.UpdateChanges();

            Console.WriteLine($"DISCOUNT REQUEST: ({request.Discount})"); //50
            Console.WriteLine($"DISCOUNT IN DECIMAL: ({request.Discount / 100m})"); //0.5

            Console.WriteLine($"PRODUCT PRICE: ({selectedProduct.OriginalPrice})"); //1000
            Console.WriteLine($"DISCOUNTED PRICE: ({selectedProduct.DiscountedPrice})"); //500
            Console.WriteLine($"PRICE DEDUCTION: ({selectedProduct.OriginalPrice * discount})"); //500

            return new DiscountedProduct_Result() { IsSuccessful = true, Message = $"{request.Discount}% discount is set to {selectedProduct.ProductName}" };
        }
    }
}
