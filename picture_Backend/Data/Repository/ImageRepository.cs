using System.Data;
using System.Threading.Tasks;
using Dapper;
using picture_Backend.Data.Context;
using picture_Backend.Domain.Model;
using picture_Backend.Models;


namespace picture_Backend{

public class ImageRepository : IImageRepository
 
    {
        private readonly ImageContext _imageContext;

        public ImageRepository(ImageContext imageContext)
        {
            _imageContext = imageContext;
        }

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
  
  public async Task<Image> GetImageByIdAsync(int id)
  {
      var query = "SELECT Id, Name, Url FROM Images Where id=@id";

      var parameters = new DynamicParameters();
      parameters.Add("Id", id, DbType.Int32);

      using var con = _imageContext.CreateConnection();


      var image = await con.QueryFirstOrDefaultAsync<Image>(query,parameters);
      return image;
  }

       public async Task<IEnumerable<Image>> GetAllImagesAsync()
        {
            var query = "SELECT Id, Name, Url FROM Images";
            using var con = _imageContext.CreateConnection();
            var image = await con.QueryAsync<Image>(query);
            return image.ToList();
        }

     
        public async Task UpdateImageAsync(int id, ImageDto image)
        {
            var query = "UPDATE Images SET Name = @Name WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Name", image.Name, DbType.String);

            using var connection = _imageContext.CreateConnection();

            await connection.ExecuteAsync(query, parameters);

        }
        
    }
}