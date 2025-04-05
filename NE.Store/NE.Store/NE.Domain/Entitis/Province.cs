using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Domain.Entitis
{
    public class Province: BaseEntity
    {
        public string ProvinceName { get; set; }
        //Thuộc tính liên kết với District (1:N)
        public List<District> Districts { get; set; } = new();

    }
}
