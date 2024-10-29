using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System.Threading.Tasks;

namespace server.Application.Utility.Services
{
    public class ImageAWS
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _amazonS3;
        public ImageAWS(IConfiguration configuration, IAmazonS3 amazons3)
        {
            _configuration = configuration;
            _amazonS3 = amazons3;
        }

        public async Task<string> UploadImageAsync(Stream imageFile, string imageName)
        {
            var bucketName = _configuration["AWS:BucketName"];
            var region = _configuration["AWS:Region"]; //ap-southeast-2

            var request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = imageName,
                InputStream = imageFile,
                ContentType = "image/png",
                CannedACL = S3CannedACL.PublicRead
            };

            await _amazonS3.PutObjectAsync(request);
            return $"https://{bucketName}.s3.{region}.amazonaws.com/{imageName}.png"; // URL for the uploaded image
        }
    }
}
