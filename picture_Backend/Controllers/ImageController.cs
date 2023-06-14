using Microsoft.AspNetCore.Mvc;
using picture_Backend.Models;
using picture_Backend.Services;

namespace picture_Backend
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Image image)
        {
            try
            {
                await _imageService.AddImageAsync(image.Name, image.Url);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}