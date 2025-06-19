using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NE.Application.Services.Interfaces;

namespace NE.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageFileController : ControllerBase
    {
        private readonly IImageFileService _imageFileService;

        public ImageFileController(IImageFileService imageFileService)
        {
            _imageFileService = imageFileService;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, int productColorId, int productId, int colorId)
        {
            await _imageFileService.UploadFileAsync(file, productColorId, productId, colorId);
            return Ok();
        }

        [HttpGet("Download")]
        public async Task<IActionResult> Download(int id)
        {
            var fileDto = await _imageFileService.DownloadFile(id);
            if (fileDto == null)
            {
                return NotFound();
            }
            //return File(fileDto.FileData, fileDto.ContentType, fileDto.FileName);//Tải về
            return File(fileDto.FileData, fileDto.ContentType);     //Show ảnh

        }

        [HttpGet]
        public async Task<IActionResult> GetListFile()
        {
            var files = await _imageFileService.GetAllFileAsync();
            return Ok(files);
        }
    }
}
