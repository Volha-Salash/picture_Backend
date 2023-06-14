using System.Threading.Tasks;
using picture_Backend.Models;

namespace picture_Backend
{
    public interface IImageRepository
    {
        Task AddImageAsync(Image image);
    }
}
