using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Domain.Entitis
{
    public class ImageFile: BaseEntity
    {
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public string ContentType { get; set; }

        public int ProductColorId { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }

        public ProductColor ProductColor { get; set; } 
        public Product Product { get; set; } 
        public Color Color { get; set; }
    }
}
