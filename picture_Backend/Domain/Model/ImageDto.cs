using picture_Backend.Models;

namespace picture_Backend.Domain.Model
{
    public class ImageDto
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
       
  
        
    }
}