using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using picture_Backend.Domain.Model;
using Microsoft.AspNetCore.Http;
using picture_Backend.Domain.Model;
using picture_Backend.Models;

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
    /*    [HttpPost]
        public string UploadImage([FromForm]IFormFile file)
        {
            try
            {
                // getting file original name
                string FileName = file.FileName;

                // combining GUID to create unique name before saving in wwwroot
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + FileName;

                // getting full path inside wwwroot/images
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", FileName);
        
                // copying file
                file.CopyTo(new FileStream(imagePath, FileMode.Create));

                return "File Uploaded Successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        */
     
        [HttpPost]
        public ActionResult Image([FromForm]ImageDto imageDto)
        {
            // Getting Name
            string name = imageDto.Name;

            // Getting Image
            var image = imageDto.Image;
            
            var filePath = Path.Combine("wwwroot/images", image.FileName);

            // Saving Image on Server
            if (image.Length > 0) {
                using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                    image.CopyTo(fileStream);
                }
            }

            return Ok(new { status = true, message = "image Posted Successfully"});
        }
/*
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
        */

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