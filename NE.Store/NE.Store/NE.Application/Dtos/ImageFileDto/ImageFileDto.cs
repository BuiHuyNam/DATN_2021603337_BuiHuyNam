using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.ImageFileDto
{
    public class ImageFileDto
    {
        public int Id { get; set; }
        public int ProductColorId { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public string ContentType { get; set; }
    }
}
