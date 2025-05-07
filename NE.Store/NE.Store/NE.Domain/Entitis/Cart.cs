using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Domain.Entitis
{
    public class Cart :BaseEntity
    {
        public int Quatity { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get;set;}
        public int ColorId { get; set; } // Thêm ColorId


    }
}
