using System.Threading.Tasks;
using picture_Backend.Domain.Model;
using picture_Backend.Models;

namespace picture_Backend
{
    public interface IImageRepository
    {
            // Task AddImageAsync(Image image);

            Task<IEnumerable<Image>> GetAllImagesAsync();
            Task<Image> GetImageByIdAsync(int id);
            Task CreateImage(ImageDto imageDto);
            Task UpdateImageName(int id, string newName);
            Task<bool> DeleteImage(int id);



    }
}
