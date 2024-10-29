using FluentValidation;
using MediatR;
using server.Application.Interfaces;
using static server.Core.ResponseModels;
using server.Application.Models;

namespace server.Application.Command_Operations.Products
{
    public class CreateProduct_Command : IRequest<CreateProduct_Result>
    {
        public string? ProductName { get; set; }
        public string? ProductDetails { get; set; }
        public string? Type { get; set; }
        public string? MainImageURL { get; set; }
        public int Price { get; set; }
        public string? SubImageOne { get; set; }
        public string? SubImageTwo { get; set; }
        public string? SubImageThree { get; set; }

        //public IFormFile? MainImageURL { get; set; }
    }

    public class CreateProduct_CommandHandler : IRequestHandler<CreateProduct_Command, CreateProduct_Result>
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<CreateProduct_Command> _validator;
        //private readonly IProductImageService _productImageService;

        public CreateProduct_CommandHandler(IProductRepository productRepository, IValidator<CreateProduct_Command> validator)
        {
            _productRepository = productRepository;
            _validator = validator;
            //_productImageService = productImageService;
        }

        public async Task<CreateProduct_Result> Handle(CreateProduct_Command request, CancellationToken ct)
        {
            var result = await _validator.ValidateAsync(request, ct);

            if (!result.IsValid)
            {
                return new CreateProduct_Result()
                {
                    IsExisting = true,
                    Message = string.Join(", ", result.Errors.Select(e => e.ErrorMessage))
                };
            }
            

            //Upload the image in the AWS S3 and return the public Object URL of the uploaded image.
            //string? mainImageURL = await _productImageService.UploadImageAsync(request.MainImageURL!, request.ProductName + "-main");

            //Create an instance of the product including the Object URL as the link of `MainImageURL` of the specific product.
            Product product = new Product(request.ProductName!, request.ProductDetails!, request.Type!, request.MainImageURL!, request.Price);

            //Adding the product to the `Products` table(DB).
            _productRepository.CreateProductAsync(product);

            //Adding the subImages of the specific product to the `ProductSubImages` table(DB).
            _productRepository.CreateProductSubImagesAsync(new ProductSubImages(product, request.SubImageOne!, request.SubImageTwo!, request.SubImageThree!));

            await _productRepository.UpdateChanges();
            
            return new CreateProduct_Result() { IsExisting = false, Message = $"Product({product.ProductName}) created successfully!" };
        }
    }
}
