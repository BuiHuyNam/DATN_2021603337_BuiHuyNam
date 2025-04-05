using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Domain.Entitis
{
    public class District:BaseEntity
    {
        public string DistrictName { get; set; }
        public int ProvinceId { get; set; }
        public Province Province { get; set; }

        //Thuộc tính liên kết với Ward (1:N)
        public List<Ward> Wards { get; set; } = new();
    }
}
