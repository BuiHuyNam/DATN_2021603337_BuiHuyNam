using NE.Application.Dtos.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.OrderDetailDto
{
    public class OrderDetailViewDto
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double TotalMoney { get; set; }
        public int ColorId { get; set; }

        public int ProductId { get; set; }
        public int OrderId { get; set; }

        public ProductViewDto Product { get; set; }
    }
}
