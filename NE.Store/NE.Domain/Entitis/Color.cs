using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Domain.Entitis
{
    public class Color:BaseEntity
    {
        public string ColorName { get; set; }

        public List<ProductColor> ProductColors { get; set; } = new();
        public List<ImageFile> ImageFiles { get; set; } = new();


    }
}
