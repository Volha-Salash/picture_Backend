using System.Net.Mime;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using picture_Backend.Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using picture_Backend.Data.Context;
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
            var images = await _imageRepository.GetAllImagesAsync();

            return Ok(images);

        }

        [HttpGet("{id}", Name = "imageById")]
        public async Task<IActionResult> GetImageById([FromRoute] int id)
        {
            var image = await this._imageRepository.GetImageByIdAsync(id);

            return Ok(image);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateImage([FromForm]ImageDto imageDto)
        {
            await _imageRepository.CreateImage(imageDto);
            return Ok();
        }
        
        
        [HttpPut("updateName/{id}/{newName}")]
        public async Task<IActionResult> UpdateImageName(int id, string newName)
        {
            var image = await _imageRepository.GetImageByIdAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            await _imageRepository.UpdateImageName(id, newName);

            return Ok();
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
        /*
           [HttpPost]
           public ActionResult Image([FromForm]ImageDto imageDto)
           {
               // Getting Name
              // string name = imageDto.Name;
               string name = Guid.NewGuid().ToString + "_" + imageDto.Name;
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
           */
       
      }
}