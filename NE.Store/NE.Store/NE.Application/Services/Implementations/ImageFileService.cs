using Microsoft.AspNetCore.Http;
using NE.Application.Dtos.ImageFileDto;
using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;
using NE.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Implementations
{
    public class ImageFileService : IImageFileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImageFileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ImageFileDto> DownloadFile(int id)
        {
            var image = await _unitOfWork.ImageFiles.GetByIdAsync(id);
            if (image == null)
            {
                return null;
            }
            return new ImageFileDto
            {
                ProductColorId = image.ProductColorId,
                ProductId = image.ProductId,
                ColorId = image.ColorId,
                FileName = image.FileName,
                FileData = image.FileData,
                ContentType = image.ContentType

            };
        }

        public async Task<List<ImageFileDto>> GetAllFileAsync()
        {
            var files = await _unitOfWork.ImageFiles.GetAllFileAsync();
            return files.Select(f => new ImageFileDto
            {
                Id = f.Id,
                ProductColorId = f.ProductColorId,
                ProductId = f.ProductId,
                ColorId =f.ColorId,
                FileName = f.FileName,
                FileData = f.FileData,
                ContentType = f.ContentType,
            }).ToList();
        }

        public async Task UploadFileAsync(IFormFile file, int productColorId, int productId, int colorId)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var image = new ImageFile
            {
                ProductColorId = productColorId,
                ProductId = productId,
                ColorId = colorId,
                FileName = file.FileName,
                FileData = memoryStream.ToArray(),
                ContentType = file.ContentType
            };
            await _unitOfWork.ImageFiles.AddAsync(image);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
