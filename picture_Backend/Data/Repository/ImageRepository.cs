using System.Data;
using System.Threading.Tasks;
using Dapper;
using picture_Backend.Data.Context;
using picture_Backend.Domain.Model;
using picture_Backend.Models;
using System.IO;
using System.Reflection;
using System.Text;


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
            var query = "SELECT Id, Name, Url FROM Images";
            using var con = _imageContext.CreateConnection();
            var image = await con.QueryAsync<Image>(query);
            return image.ToList();
        }

        public async Task CreateImage(ImageDto imageDto)
        {
            string name = Guid.NewGuid().ToString() + "_" + imageDto.Name;
            var image = imageDto.Image;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", image.FileName);

            if (image.Length > 0)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
            }

            using (var connection = _imageContext.CreateConnection())
            {
                var query = @"INSERT INTO Images (Name, Url) VALUES (@Name, @Url)";
                var parameters = new { Name = name, Url = filePath };

                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<Image> GetImageByIdAsync(int id)
        {
            var query = "SELECT Id, Name, Url FROM Images Where id=@id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);

            using var con = _imageContext.CreateConnection();


            var image = await con.QueryFirstOrDefaultAsync<Image>(query, parameters);
            return image;
        }

        public async Task UpdateImageName(int id, string newName)
        {
            using (var connection = _imageContext.CreateConnection())
            {
                var query = @"UPDATE Images SET Name = @NewName, Url = REPLACE(Url, @OldName, @NewName) WHERE Id = @Id";
                var parameters = new { NewName = newName, OldName = GetImageName(id), Id = id };

                await connection.ExecuteAsync(query, parameters);
            }
        }

        private string GetImageName(int id)
        {
            using (var connection = _imageContext.CreateConnection())
            {
                var query = @"SELECT Name FROM Images WHERE Id = @Id";
                var result = connection.Query<string>(query, new { Id = id }).FirstOrDefault();

                return result;
            }
        }

        public async Task DeleteImage(int id)
        {
            using (var connection = _imageContext.CreateConnection())
            {
                var query = @"DELETE FROM Images WHERE Id = @Id";
                var parameters = new { Id = id };

                await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}





/*     
public async Task<Image> CreateImageAsync(ImageDto imageDto)
{
    if (string.IsNullOrWhiteSpace(imageDto.Data))
        throw new ArgumentException("Image data is required", nameof(imageDto.Data));
    
    //byte[] dataBytes = Encoding.UTF8.GetBytes(image.Data);

    var query = "INSERT INTO Images (Name, Url) VALUES (@Name, @Url)";

    var parameters = new DynamicParameters();
    parameters.Add("Name", imageDto.Name, DbType.String);

    var imagePath = @"C:\Users\volha.salash\Pictures/" + imageDto.Name;
    using (var fileStream = new FileStream(imagePath, FileMode.Create))
    {
        var imageData = Convert.FromBase64String(imageDto.Data);
        await fileStream.WriteAsync(imageData, 0, imageData.Length);
    }
    var imageUrl = "http://localhost/api/image/" + imageDto.Name;
    parameters.Add("Url", imageUrl, DbType.String);

    using var connection = _imageContext.CreateConnection();

    var id = await connection.QuerySingleAsync<int>(query, parameters);

    var createdImage = new Image
    {
        Id = id,
        Name = imageDto.Name,
        Url = imageUrl
    };

    return createdImage;
}
*/

/*
  public async Task<Image> CreateImageAsync(ImageDto image)
        {
            var query = "INSERT INTO Images (Name, Url) VALUES (@Name, @Url)";

            var parameters = new DynamicParameters();
            parameters.Add("Name", image.Name, DbType.String);
            parameters.Add("Url", image.Url, DbType.String);

            using var connection = this._imageContext.CreateConnection();

            var id = await connection.QuerySingleAsync<int>(query, parameters);

            var createdImage = new Image
            {
                Id = id,
                Name = image.Name,
                Url = image.Url
            };

            return createdImage;
        }  
        
  */
        /*
      public async Task UpdateImageAsync(int id, ImageDto image)
      {
          var query = "UPDATE Images SET Name = @Name WHERE Id = @Id";
   
          var parameters = new DynamicParameters();
          parameters.Add("Id", id, DbType.Int32);
          parameters.Add("Name", image.Name, DbType.String);
   
          using var connection = _imageContext.CreateConnection();
   
          await connection.ExecuteAsync(query, parameters);
   
      }
      */
      