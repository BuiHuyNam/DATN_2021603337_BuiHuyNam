using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Domain.Entitis
{
    public class Order:BaseEntity
    {
        public int Status { get; set; }
        public string? Note { get; set; }
        public double TotalMoney { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public int? CouponId { get; set; }
        public Coupon Coupon { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new();


    }
}
