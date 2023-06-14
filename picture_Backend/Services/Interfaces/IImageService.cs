
namespace picture_Backend.Services
{
    public interface IImageService
    {
        Task AddImageAsync(string name, string url);
    }
}