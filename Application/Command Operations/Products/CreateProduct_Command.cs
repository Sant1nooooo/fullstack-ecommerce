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
        public string? ImageURL { get; set; }
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
        //private readonly ImageAWS _imageAWS;

        public CreateProduct_CommandHandler(IProductRepository productRepository, IValidator<CreateProduct_Command> validator)
        {
            _productRepository = productRepository;
            _validator = validator;
            //_imageAWS = imageAWS;
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

            //string? mainImageURL = await _imageAWS.UploadImageAsync(request.MainImageURL!.OpenReadStream(), request.ProductName + "-main");
            Product product = new Product(request.ProductName!, request.ProductDetails!, request.Type!, request.ImageURL!, request.Price);

            //Adding the product
            _productRepository.CreateProductAsync(product);
            
            //Adding the subImages.
            _productRepository.CreateProductSubImagesAsync(new ProductSubImages(product, request.SubImageOne!, request.SubImageTwo!, request.SubImageThree!));
            await _productRepository.UpdateChanges();
            
            return new CreateProduct_Result() { IsExisting = false, Message = $"Product({product.ProductName}) created successfully!" };
        }
    }
}
