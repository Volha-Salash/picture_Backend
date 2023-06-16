using Microsoft.AspNetCore.Mvc;
using picture_Backend.Domain.Model;
using Microsoft.AspNetCore.Http;
using picture_Backend.Domain.Model;

namespace picture_Backend
{
    //[Route("api/images")]
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        public ImageController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpGet]
       public async Task<IActionResult> GetImagesAsync()
        {
            var images= await _imageRepository.GetAllImagesAsync();

            return Ok(images);

        }
        
      
        [HttpGet("{id}",Name = "imageById")]
        public async Task<IActionResult> GetImageById([FromRoute]int id)
        {
            var image = await this._imageRepository.GetImageByIdAsync(id);

            return Ok(image);
        }

        [HttpPost]
        public async Task<IActionResult> CreateImage(ImageDto image)
        {
            try
            {
                var createdImage = await this._imageRepository.CreateImageAsync(image);
                return CreatedAtRoute("ImageById", new { id = createdImage.Id }, createdImage);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateImage(int id, ImageDto image)
        {
            try
            {
                var dbImage = await _imageRepository.GetImageByIdAsync(id);
                if (dbImage == null)
                    return NotFound();

                await _imageRepository.UpdateImageAsync(id, image);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

    }
}