using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.OrderDetailDto
{
    public class OrderDetailUpdateDto
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double TotalMoney { get; set; }
    }
}
