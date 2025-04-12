using NE.Application.Dtos.ColorDto;
using NE.Application.Dtos.ImageFileDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.ProductColorDto
{
    public class ProductColorViewDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ColorId { get; set; }
        public int ProductId { get; set; }
        public ColorViewDto Color { get; set; }
        public List<ImageFileDto.ImageFileDto> ImageFiles { get; set; } = new();



    }
}
