using System.Data;
using System.Threading.Tasks;
using Dapper;
using picture_Backend.Data.Context;
using picture_Backend.Domain.Model;
using picture_Backend.Models;
using System.IO;
using System.Reflection;
using System.Text;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;


namespace picture_Backend
{
    public class ImageRepository : IImageRepository

    {
        private readonly ImageContext _imageContext;

        public ImageRepository(ImageContext imageContext)
        {
            _imageContext = imageContext;
        }

        public async Task<IEnumerable<Image>> GetAllImagesAsync()
        {
            var connection = _imageContext.CreateConnection();
            var images = await connection.GetAllAsync<Image>();
            return images;
        }

        public async Task CreateImage(ImageDto imageDto)
        {
            string name = $"{Guid.NewGuid()}_{imageDto.Name}";
            var image = imageDto.Image;
            
            var filePath = Path.Combine(@"wwwroot\images", image.FileName);
            if (image.Length > 0)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
            }

            var imageEntity = new Image
            {
                Name = name,
                Url = "/images/" + image.FileName
            };
            var connection = _imageContext.CreateConnection();
            await connection.InsertAsync(imageEntity);
        }
       

        public async Task<Image> GetImageByIdAsync(int id)
        {
            var connection = _imageContext.CreateConnection();
            var image = await connection.GetAsync<Image>(id);
            return image;
        }
       

        public async Task UpdateImageName(int id, string newName)
        {
            var connection = _imageContext.CreateConnection();
            var image = await connection.GetAsync<Image>(id);
            if (image != null)
            {
                image.Name = newName;
                await connection.UpdateAsync(image);
            }
        }

       
        public async Task <bool> DeleteImage(int id)
        {
            var connection = _imageContext.CreateConnection();
            var image = await connection.GetAsync<Image>(id);
            if (image != null)
            {
                await connection.DeleteAsync(image);
                
                return true;
            }

            return false;
        }
    }
}
