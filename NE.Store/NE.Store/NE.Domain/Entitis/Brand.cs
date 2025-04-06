using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Domain.Entitis
{
    public class Brand: BaseEntity
    {
        public  string BrandName { get; set; }
        public bool? IsActive { get; set; }

        //Thuộc tính liên kết với Product (1:N)
        public List<Product> Products { get; set; } = new();
    }
}
