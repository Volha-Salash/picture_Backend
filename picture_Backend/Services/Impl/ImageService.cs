using picture_Backend.Models;
using picture_Backend.Services;

namespace picture_Backend
{

    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public async Task AddImageAsync(string name, string url)
        {
            var image = new Image { Name = name, Url = url };
            await _imageRepository.AddImageAsync(image);
        }
    }
}