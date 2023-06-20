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
        public async Task<IActionResult> CreateImage([FromForm] ImageDto imageDto)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage([FromRoute] int id)
        {
            await _imageRepository.DeleteImage(id);

            return Ok();
        }
    }
    }