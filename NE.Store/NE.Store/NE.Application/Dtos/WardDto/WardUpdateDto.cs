using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.WardDto
{
    public class WardUpdateDto
    {
        public int Id { get; set; }
        public string WardName { get; set; }
        public int DistrictId { get; set; }
    }
}
