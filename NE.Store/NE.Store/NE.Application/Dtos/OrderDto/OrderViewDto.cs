using NE.Application.Dtos.OrderDetailDto;
using NE.Application.Dtos.UserDto;
using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.OrderDto
{
    public class OrderViewDto
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string? Note { get; set; }
        public double TotalMoney { get; set; }

        public int UserId { get; set; }

        public int? CouponId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public UserViewDto User { get; set; }

        public List<OrderDetailViewDto> OrderDetails { get; set; } = new();
       

       


    }
}
