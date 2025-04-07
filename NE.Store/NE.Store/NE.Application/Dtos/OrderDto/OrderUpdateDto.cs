using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.OrderDto
{
    public class OrderUpdateDto
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string? Note { get; set; }
        public double TotalMoney { get; set; }


    }
}
