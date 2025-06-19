using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Domain.Entitis
{
    public class Coupon:BaseEntity
    {
        public string Code { get; set; }
        public float Discount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime UseDate { get; set; }
        public int Quantity { get; set; }

        public List<Order> Orders { get; set; } = new();

    }
}
