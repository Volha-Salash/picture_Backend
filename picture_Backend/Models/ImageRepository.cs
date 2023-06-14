using System.Threading.Tasks;
using picture_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace picture_Backend{

public class ImageRepository : IImageRepository
{
    private readonly AppDbContext _dbContext;

    public ImageRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddImageAsync(Image image)
    {
        await _dbContext.Images.AddAsync(image);
        await _dbContext.SaveChangesAsync();
    }
}

}