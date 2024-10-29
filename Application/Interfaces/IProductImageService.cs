namespace server.Application.Interfaces
{
    public interface IProductImageService
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string productName);
    }
}
