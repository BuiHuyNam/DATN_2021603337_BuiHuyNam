using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.CouponDto
{
    public class CouponUpdateDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public float Discount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime UseDate { get; set; }
        public int Quantity { get; set; }
    }
}
