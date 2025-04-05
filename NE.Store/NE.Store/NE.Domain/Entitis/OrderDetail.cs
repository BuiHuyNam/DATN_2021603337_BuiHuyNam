using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Domain.Entitis
{
    public class OrderDetail:BaseEntity
    {
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double TotalMoney { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

    }
}
