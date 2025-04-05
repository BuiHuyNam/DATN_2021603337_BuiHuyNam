using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.DistrictDto
{
    public class DistrictViewDto
    {
        public int Id { get; set; }
        public string DistrictName { get; set; }
        public int ProvinceId { get; set; }
    }
}
