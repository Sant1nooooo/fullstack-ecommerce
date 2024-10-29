using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using server.Application.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace server.Application.Utility.Services
{
    public class ProductImageService : IProductImageService
    {
        //`Configuration` dependency will be use to access the appsettings of the .NET project through dependency injection when this service class is added 
        //to the dependency containeer of the project using .AddSingleton<>();
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _amazonS3;
        public ProductImageService(IConfiguration configuration, IAmazonS3 amazons3)
        {

            _configuration = configuration;
            _amazonS3 = amazons3;
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile, string imageName)
        {
            var bucketName = _configuration["AWS:BucketName"];
            var region = _configuration["AWS:Region"]; //ap-southeast-2

            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = imageName,
                ContentType = imageFile.ContentType,
                InputStream = imageFile.OpenReadStream(),
            };

            await _amazonS3.PutObjectAsync(request);
            return $"https://{bucketName}.s3.{region}.amazonaws.com/{imageName}.png"; // URL for the uploaded image
        }
    }
}
