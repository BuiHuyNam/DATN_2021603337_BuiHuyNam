using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.OrderDto
{
    public class OrderCreateDto
    {
        public int Status { get; set; }
        public string? Note { get; set; }
        public double TotalMoney { get; set; }

        public int UserId { get; set; }
        public int? CouponId { get; set; }
    }
}
