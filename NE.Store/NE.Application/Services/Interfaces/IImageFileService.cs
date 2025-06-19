using Microsoft.AspNetCore.Http;
using NE.Application.Dtos.ImageFileDto;
using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Interfaces
{
    public interface IImageFileService
    {
        Task UploadFileAsync(IFormFile file, int productColorId, int productId, int colorId);
        Task<ImageFileDto> DownloadFile(int id);
        Task<List<ImageFileDto>> GetAllFileAsync();
    }
}
