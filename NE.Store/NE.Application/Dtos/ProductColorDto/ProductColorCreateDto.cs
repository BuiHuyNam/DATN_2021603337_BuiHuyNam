using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.ProductColorDto
{
    public class ProductColorCreateDto
    {
        public int Quantity { get; set; }
        public int ColorId { get; set; }
        public int ProductId { get; set; }
    }
}
